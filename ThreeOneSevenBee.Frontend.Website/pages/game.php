<?
$_scripts[] = "js/bridge.js";
$_scripts[] = "js/fastclick.js";
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
    <canvas id="canvas" width="600" height="400" style="-webkit-tap-highlight-color:rgba(0,0,0,0); z-index: 1"></canvas>
    <input id="input" style="position: absolute; left: -500px; top: -500px; z-index: 0" type="text" />
	<script>
	if ('addEventListener' in document) {
    document.addEventListener('DOMContentLoaded', function() {
        FastClick.attach(document.body);
    }, false);
}
	</script>
<? require('footer.php'); ?>
