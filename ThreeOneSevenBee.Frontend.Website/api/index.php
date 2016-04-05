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
        echo ("{".
                "success: ".($success ? "true" : "false").
                ($data    != null ? ",data: ".json_encode($data) : "").
                ($message != null ? ",message: \"".$message."\"" : "").
              "}");
        exit();
    }

    static function user_logout($IN, $db) {
        session_start();
        if(isset($_SESSION['authorized']))
            unset($_SESSION['authorized']);
        API::respond();
    }

    static function get_authorized_user($IN, $db) {
        session_start();
        if(isset($_SESSION['authorized'])) {
            if($db->query("SELECT * FROM gamedb.user AS user WHERE user.id=?", $_SESSION['authorized'])) {
                respond(true, $db->fetch());                
            }
            respond(false, null, "No user found");
        }
        else {
            respond(false, null, "No user logged in");
        }
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
}

?>