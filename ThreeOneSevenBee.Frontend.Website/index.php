<?
ob_start();
session_start();
header('Content-Type: text/html; charset=utf-8');

require('db.php');
require('helpers.php');

$requestURI = urldecode($_SERVER["REQUEST_URI"]); 
// test if file was requested but does not exsist
if(preg_match("/\..+$/", $requestURI)) {
    header("HTTP/1.0 404 Not Found");
    require("404.php");
    die();
}
// split request uri into parts
preg_match("/^\/([^?]*)\??(.*?)$/", $requestURI, $elm);
if(strlen($elm[1]) == 0) {
    $elm[1] = "forside";
}


if(strlen($elm[0]) <= 1) {
    $elm[1] = "game";
}

$db = new DB();
$_scripts = array();

@include('pages/'.$elm[1].'.php');
?>