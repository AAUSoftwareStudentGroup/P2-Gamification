<?
requireAuth();

$_scripts[] = "js/bridge.js";
$_scripts[] = "js/threeOneSevenBee.model.euclidean.js";
$_scripts[] = "js/threeOneSevenBee.model.uI.js";
$_scripts[] = "js/threeOneSevenBee.model.expression.js";
$_scripts[] = "js/threeOneSevenBee.model.expression.expressionRules.js";
$_scripts[] = "js/threeOneSevenBee.model.collections.js";
$_scripts[] = "js/threeOneSevenBee.model.expression.expressions.js";
$_scripts[] = "js/threeOneSevenBee.model.js";
$_scripts[] = "js/threeOneSevenBee.model.game.js";
$_scripts[] = "js/threeOneSevenBee.frontend.js";
$_scripts[] = "js/threeOneSevenBee.model.geometry.js";
$_scripts[] = "js/threeOneSevenBee.model.polygon.js";
?>
<? require('header.php'); ?>

    <canvas id="canvas" width="600" height="400"></canvas>
    <a href="#" onclick="$.get('/api?action=user_logout').done(function() {location ='/'});">Log ud</a>

<? require('footer.php'); ?>
