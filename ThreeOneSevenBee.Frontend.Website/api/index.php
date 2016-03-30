<?
if(isset($_GET['action']))
    $_GET['action']();
elseif(isset($_POST['action']))
    $_POST['action']();

function respond($success = true, $message = null) {
    // echo ("hello");
    echo ("{success: ".($success ? "true" : "false").($message != null ? ",message: \"".$message."\"" : "")."}");
}

function userlogout() {
    session_start();
    if(isset($_SESSION['authorized']))
        unset($_SESSION['authorized']);
    respond();
}


?>