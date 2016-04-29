<?
// $_DEBUG = false;
$_DEBUG = true;
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
    if(strcmp($IN['debug'], "1") == 0)
        $_SESSION['authorized'] = 5; // Tanner helland
}

if(isset($IN['token']) && $IN['token'] != "NULL") {
    $db->query("SELECT user.id 
                FROM user 
                WHERE session_token=?",
                $IN['token']
            );
    if($row = $db->fetch()) {
        $_SESSION['authorized'] = $row['id'];
    }
    else
        unset($_SESSION['authorized']);
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

                $return = array(
                    "token" => $session_token,
                    "id" => $row['id']
                );
                API::respond(true, $return);
            }
        }
        API::respond(false, "Username or password was incorrect");
    }

    static function user_logout($IN, $db) {
        if(isset($_SESSION['authorized']) && $_SESSION['authorized'] > 0) {
            $db->query("UPDATE user
                        SET session_token=NULL
                        WHERE id=?;",
                        $_SESSION['authorized']
                    );
            unset($_SESSION['authorized']);
            API::respond();
        }
        API::respond(false, null, "Not authorized");
    }

    static function get_users($IN, $db) {
        if(isset($_SESSION['authorized']) && $_SESSION['authorized'] > 0) {

            $result = array();
            $user_class_id = 0;
            $db->query("SELECT user.class_id FROM user WHERE user.id=?",
                        (int)$_SESSION['authorized']);
            

            if($row = $db->fetch())
                $user_class_id = $row['class_id'];
            else
                API::respond(false, null, "Invalid user");

            
            $db->query("SELECT 
                            user.name,
                            user.badges
                        FROM gamedb.user AS user 
                        WHERE user.deleted_at IS NULL
                        AND user.class_id=?",
                        $user_class_id);
            while($row = $db->fetch()) {
                $row['badges'] = explode(',', $row['badges']);
                $result[] = $row;
            }
            API::respond(true, $result);
        }
        API::respond(false, null, "Not authorized");
    }

    static function delete_user_by_id($IN, $db) {
        $db->query("SELECT user.id 
                    FROM gamedb.user 
                    WHERE 
                        user.id=?
                        AND user.deleted_at IS NULL;", 
                    $IN['user_id']
                );

        if($db->fetch()) {
            $db->query("UPDATE gamedb.user
                        SET deleted_at=UNIX_TIMESTAMP()
                        WHERE id=?;", $IN['user_id']
                    );
            API::respond();
        }
        API::respond(false);
    }

    static function recover_user_by_id($IN, $db) {
        $db->query("SELECT user.id 
                    FROM gamedb.user 
                    WHERE 
                        user.id=?
                        AND user.deleted_at IS NOT NULL;", 
                    $IN['user_id']
                );
        
        if($db->fetch()) {
            $db->query("UPDATE gamedb.user
                        SET deleted_at=NULL
                        WHERE id=?;", $IN['user_id']
                    );
            API::respond();
        }
    
        API::respond(false);
    }

    ///// UNDOCUMENTED DUE TO THE DAMAGE THAT CAN BE CAUSED
    ///// THE FOLLOWING ARE ONLY USED IN THE ADMIN PANEL
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
    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    static function get_levels($IN, $db) {
        if(!isset($_SESSION['authorized']))
            API::respond(false, null, "User not logged in");

        $db->query("SELECT 
                        level.id AS level_id,
                        category.name AS category_name,
                        level.initial_expression AS initial_expression, 
                        level.star_expressions AS star_expressions,
                        IFNULL(level_progress.progress,initial_expression) AS progress,
                        IFNULL(level_progress.stars,0) AS stars
                    FROM 
                        gamedb.level AS level
                    LEFT JOIN 
                        gamedb.level_category AS category ON level.level_category_id=category.id
                    LEFT JOIN 
                        gamedb.user_level_progress AS level_progress 
                        ON 
                            level_progress.level_id = level.id
                        AND 
                            level_progress.user_id=?
                    ORDER BY category.order ASC, level.order ASC;",
                    $_SESSION['authorized']
        );

        $categories = array();
        while($row = $db->fetch()) {
            $level = array(
                'id' => $row['level_id'],
                'initial_expression' => $row['initial_expression'],
                'star_expressions' => explode('|', $row['star_expressions']),
                'current_expression' => $row['progress'],
                'stars' => $row['stars']
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

    static function user_add_badge($IN, $db) {
        if(!isset($_SESSION['authorized']))
            API::respond(false, null, "User not logged in");
        
        $db->query("SELECT user.badges
                    FROM gamedb.user
                    WHERE user.id=?",
                    $_SESSION['authorized']
                );
        if($row = $db->fetch()) {
            $tmp_badges = explode(',', $row['badges']);
            $tmp_badges[] = $IN['badge_id'];
            $badges = array();

            foreach ($tmp_badges as $index => $badge) {
                if(is_numeric($badge) && is_int((int)$badge))
                    $badges[] = $badge;
            }
            $badges = array_unique($badges, SORT_NUMERIC);

            $badges = implode(',', $badges);
            $db->query("UPDATE gamedb.user
                        SET user.badges=?
                        WHERE user.id=?",
                        $badges,
                        $_SESSION['authorized']
                    );
            API::respond();
        }
        else
            API::respond(false, null, "Invalid user");
    }

    static function save_user_level_progress($IN, $db) {
        if(!isset($_SESSION['authorized']))
            API::respond(false, null, "User not logged in");
        
        $user_id  = (int)$_SESSION['authorized'];
        $level_id = (isset($IN['level_id'])           ? (int)$IN['level_id']           : null);        
        $state    = (isset($IN['current_expression']) ?      $IN['current_expression'] : null);        
        $stars    = (int)(isset($IN['stars'])         ?      $IN['stars'] : 0);        

        if($level_id != null && $state != null) {
            $db->query("DELETE FROM gamedb.user_level_progress
                        WHERE level_id=? AND user_id=?",
                        $level_id,
                        $user_id
            );
            $db->query("INSERT INTO gamedb.user_level_progress (user_id,level_id,progress,stars)
                        VALUES (?,?,?,?);",
                        $user_id,
                        $level_id,
                        $state,
                        $stars
            );
            API::respond(true, "Level: ".$level_id.", State: ".$state.", Stars: ".$stars);
        }
        else {
            API::respond(false, null, "Invalid parameters");
        }
    }

    static function is_authenticated($IN, $db) {
        API::get_current_user($IN, $db);
    }

    static function get_current_user($IN, $db) {
        if(!isset($_SESSION['authorized']))
            API::respond(false, null, "user not authorized");
        
        $db->query("SELECT 
                        user.id AS id, 
                        user.name AS name, 
                        user.session_token AS token,
                        user.badges AS badges 
                    FROM gamedb.user AS user
                    WHERE user.id = ?",
                    (isset($_SESSION['authorized']) ? $_SESSION['authorized'] : 0));
        if($result = $db->fetch()) {
            $result['badges'] = explode(',', $result['badges']);
            API::respond(true, $result);
        }
        API::respond(false, null, "Unkown error");
    }
}

?>