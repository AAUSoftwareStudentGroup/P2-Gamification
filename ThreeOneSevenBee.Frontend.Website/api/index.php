<?
$_DEBUG = false;
// $_DEBUG = true;
if($_SERVER['HTTP_HOST'] == "webmat.cs.aau.dk" || $_DEBUG == false) {
    error_reporting(0);
}

require('../helpers.php');
require('../db.php');
$db = new DB();

$IN = null;
if(isset($_GET['action']))
    $IN = $_GET;
elseif(isset($_POST['action'])) {
    $IN = $_POST;
}

session_start();

if(isset($IN['debug']) && $IN['debug'] != "NULL") {
    if($IN['debug'] == "1")
        $_SESSION['authorized'] = 5; // Tanner helland
    else {
        $db->query("SELECT user.id 
                    FROM user 
                    WHERE session_token=?",
                    $IN['debug']    
                );
        if($row = $db->fetch()) {
            $_SESSION['authorized'] = $row['id'];
        }
    }
}

if($IN != null)
    API::$IN['action']($IN, $db);
else
    API::respond(false, null, "no action given");

class API {
    static function respond($success = true, $data = null, $message = null) {
        echo ('{'.
                '"success": '.($success ? '"true"' : '"false"').
                ($data    != null ? ',"data": '.json_encode($data) : '').
                ($message != null ? ',"message": "'.$message.'"' : '').
              '}');
        die();
    }

    static function user_logout($IN, $db) {
        if(isset($_SESSION['authorized'])) {
            unset($_SESSION['authorized']);
            $db->query("UPDATE user
                            SET session_token=NULL
                            WHERE id=?;",
                            $row['id']
                        );
        }
        API::respond();
    }

    static function user_login($IN, $db) {
        $db->query("SELECT password_hash, id FROM user WHERE BINARY name = ? AND deleted_at IS NULL",
                    $IN['username']);

        if($row = $db->fetch()) {
            if(password_verify($IN['password'], $row['password_hash'])) {
                $_SESSION['authorized'] = $row['id'];
                $session_token = getRandomString(16);
                $db->query("UPDATE user
                            SET session_token=?
                            WHERE id=?;",
                            $session_token,
                            $row['id']
                        );
                API::respond(true, $session_token);
            }
        }
        API::respond(false, "Username or password was incorrect");
    }

    static function get_users($IN, $db) {
        $result = array();
        $db->query("SELECT user.name FROM gamedb.user AS user WHERE user.deleted_at IS NULL");
        while($row = $db->fetch())
            $result[] = $row;
        API::respond(true, $result);
    }

    static function delete_user_by_id($IN, $db) {
        $db->query("UPDATE gamedb.user
                      SET deleted_at=UNIX_TIMESTAMP()
                      WHERE id=?;", $IN['user_id']
                    );
        API::respond();
    }

    static function delete_class_by_id($IN, $db) {
        $db->query("UPDATE gamedb.class
                      SET deleted_at=UNIX_TIMESTAMP()
                      WHERE id=?;", $IN['class_id']
                    );
        API::respond();
    }

    static function delete_school_by_id($IN, $db) {
        $db->query("UPDATE gamedb.school
                      SET deleted_at=UNIX_TIMESTAMP()
                      WHERE id=?;", $IN['school_id']
                    );
        API::respond();
    }

    static function delete_category_by_id($IN, $db) {
        $db->query("DELETE FROM gamedb.level_category
                      WHERE id=?;", $IN['category_id']
                    );
        $db->query("DELETE FROM gamedb.level
                      WHERE level_category_id=?;", $IN['category_id']
                    );
        API::respond();
    }

    static function delete_level_by_id($IN, $db) {
        $db->query("DELETE FROM gamedb.level
                      WHERE id=?;", $IN['level_id']
                    );
        API::respond();
    }

    static function order_level_categories($IN, $db) {
        $ids = $IN['id_by_order'];
        $n = count($ids);
        for($i = 0; $i < $n; $i++) {
            $db->query("UPDATE gamedb.level_category
                        SET level_category.order = ?
                        WHERE id=?;",
                        $i, 
                        (int)$ids[$i]
                    );
        }
        API::respond(true);
    }

    static function order_levels($IN, $db) {
        $ids = $IN['id_by_order'];
        $n = count($ids);
        for($i = 0; $i < $n; $i++) {
            $db->query("UPDATE gamedb.level
                        SET level.order = ?
                        WHERE id=?;",
                        $i, 
                        (int)$ids[$i]
                    );
        }
        API::respond(true);
    }

    static function save_user_level_progress($IN, $db) {
        if(!isset($_SESSION['authorized']))
            API::respond(false, null, "User not logged in");
        
        $user_id  = (int)$_SESSION['authorized'];
        $level_id = (isset($IN['level_id'])           ? (int)$IN['level_id']           : null);        
        $state    = (isset($IN['current_expression']) ?      $IN['current_expression'] : null);        

        if($level_id != null && $state != null) {
            $db->query("DELETE FROM gamedb.user_level_progress
                        WHERE level_id=? AND user_id=?",
                        $level_id,
                        $user_id
            );
            $db->query("INSERT INTO gamedb.user_level_progress (user_id,level_id,progress)
                        VALUES (?,?,?);",
                        $user_id,
                        $level_id,
                        $state
            );
            API::respond(true, "Level: ".$level_id.", State: ".$state);
        }
        else {
            API::respond(false, null, "Invalid parameters");
        }
    }

    static function get_levels($IN, $db) {
        if(!isset($_SESSION['authorized']))
            API::respond(false, null, "User not logged in");

        $db->query("SELECT 
                        level.id AS level_id,
                        category.name AS category_name,
                        level.initial_expression AS initial_expression, 
                        level.star_expressions AS star_expressions,
                        IFNULL(level_progress.progress,initial_expression) AS progress
                    FROM 
                        gamedb.level AS level
                    LEFT JOIN 
                        gamedb.level_category AS category ON level.level_category_id=category.id
                    LEFT JOIN 
                        gamedb.user_level_progress AS level_progress ON level_progress.level_id = level.id
                    WHERE 
                        level_progress.user_id=5 OR level_progress.user_id IS NULL
                    ORDER BY category.order ASC, level.order ASC;",
                    $_SESSION['authorized']
        );

        $categories = array();
        while($row = $db->fetch()) {
            $level = array(
                'id' => $row['level_id'],
                'initial_expression' => $row['initial_expression'],
                'star_expressions' => explode('|', $row['star_expressions']),
                'current_expression' => $row['progress']
            );

            $cat_name = $row['category_name'];
            $cat_exsists = false;
            foreach ($categories as $index => $cat) {
                if(strcmp($cat['name'], $cat_name) == 0) {
                    $categories[$index]['levels'][] = $level;
                    $cat_exsists = true;
                }
            }
            if($cat_exsists == false) {
                $categories[] = array(
                    'name' => $cat_name,
                    'levels' => array($level)
                );
            }
            
        }
        API::respond(true, $categories);
    }

    static function get_current_user($IN, $db) {
        $db->query("SELECT user.name AS token FROM gamedb.user AS user
                    WHERE user.id = ?",
                    (isset($_SESSION['authorized']) ? $_SESSION['authorized'] : 0));
        if($result = $db->fetch())
            API::respond(true, $result);
        API::respond(false, null, "user not authorized");
    }

    static function set_current_user($IN, $db) {
        if(isset($IN['id']) && is_int((integer)$IN['id'])) {
            $_SESSION['authorized'] = $IN['id'];
            API::respond();
        }
        API::respond(false, null, "No id set");
    }
}

?>