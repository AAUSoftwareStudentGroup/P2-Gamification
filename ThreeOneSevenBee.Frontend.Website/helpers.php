<?
function isAuthed() {
    if(!isset($_SESSION['authorized']) && $_SESSION['authorized'] != true)
        return true;
    return false;
}

function requireAuth() {
    if(!isset($_SESSION['authorized']) && $_SESSION['authorized'] != true)
        header("location: /login");
}

function getRandomString($length = 6) {
    $characters = 'abcdefghijklmnopqrstuvwxyz';
    $string = '';
    for ($i = 0; $i < $length; $i++) {
        $string .= $characters[rand(0, strlen($characters) - 1)];
    }
    return $string;
}