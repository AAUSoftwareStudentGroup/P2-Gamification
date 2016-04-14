<?
$_DEBUG = false;
// $_DEBUG = true;
if($_SERVER['HTTP_HOST'] != "localhost" || !$_DEBUG) {
    error_reporting(0);
}

require('../db.php');
$db = new DB();

$IN = null;
if(isset($_GET['action']))
    $IN = $_GET;
elseif(isset($_POST['action']))
    $IN = $_POST;

if(isset($IN['debug']) && $IN['debug'] == 1) {
    session_start();
    $_SESSION['authorized'] = 5; // Tanner helland
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
        session_start();
        if(isset($_SESSION['authorized']))
            unset($_SESSION['authorized']);
        API::respond();
    }

    static function get_users($IN, $db) {
        $result = array();
        $db->query("SELECT user.name FROM gamedb.user AS user");
        while($row = $db->fetch())
            $result[] = $row;
        API::respond(true, $result);
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

    static function get_levels($IN, $db) {
        $db->query("SELECT 
                        category.name AS category_name,
                        level.initial_expression AS initial_expression, 
                        level.star_expressions AS star_expressions
                    FROM 
                        gamedb.level AS level
                    LEFT JOIN 
                        gamedb.level_category AS category ON level.level_category_id=category.id
                    ORDER BY level.level_category_id ASC;"
                   );
        $categories = array();
        while($row = $db->fetch()) {
            $level = array(
                'initial_expression' => $row['initial_expression'],
                'star_expressions' => explode('|', $row['star_expressions'])
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
        session_start();
        $db->query("SELECT user.name FROM gamedb.user AS user
                    WHERE user.id = ?",
                    (isset($_SESSION['authorized']) ? $_SESSION['authorized'] : 0));
        if($result = $db->fetch())
            API::respond(true, $result);
        API::respond(false, null, "user not authorized");
    }

    static function set_current_user($IN, $db) {
        session_start();
        if(isset($IN['id']) && is_int((integer)$IN['id'])) {
            $_SESSION['authorized'] = $IN['id'];
            API::respond();
        }
        API::respond(false, null, "No id set");
    }
}

?>