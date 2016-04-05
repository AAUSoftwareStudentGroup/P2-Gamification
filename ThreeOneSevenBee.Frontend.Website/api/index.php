<?
require('../db.php');
$db = new DB();

$IN = null;
if(isset($_GET['action']))
    $IN = $_GET;
elseif(isset($_POST['action']))
    $IN = $_POST;

if($IN != null)
    API::$IN['action']($IN, $db);
else
    respond(false, null, "no action given");

class API {
    static function respond($success = true, $data = null, $message = null) {
        echo ('{'.
                '"success": '.($success ? '"true"' : '"false"').
                ($data    != null ? ',"data": '.json_encode($data) : '').
                ($message != null ? ',"message": "'.$message.'"' : '').
              '}');
        exit();
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
        $levels = array();
        while($row = $db->fetch()) {
            $level = array(
                'initial_expression' => $row['initial_expression'],
                'star_expressions' => $row['star_expressions']
            );
            if(isset($levels[ $row['category_name'] ]))
                $levels[ $row['category_name'] ][] = $level;
            else
                $levels[ $row['category_name'] ] = array($level);
        }
        API::respond(true, $levels);
    }
}

?>