<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>317B</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>

    <? foreach ($_scripts as $script): ?>
    <script src="<?= $script ?>"></script>
    <? endforeach; ?>

    <link rel="stylesheet" type="text/css" href="/css.css">
    
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
</head>
<body <?= ($elm[1] == 'game' ? 'style="overflow:hidden"' : '') ?>>