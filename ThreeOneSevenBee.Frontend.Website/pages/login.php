<?
    if(isset($_POST) && !empty($_POST)) {
        $db->query("SELECT password_hash, id FROM user WHERE BINARY name = ?",
                    $_POST['username']);

        if($row = $db->fetch()) {
            if(password_verify($_POST['password'], $row['password_hash'])) {
                
                $_SESSION['authorized'] = $row['id'];
                header("location: /");
            }
        }
    }
?>

<? require('header.php'); ?>
    <form action="/login" method="POST" class="mw300 center">
        <input type="text" name="username" placeholder="Brugernavn" />
        <input type="password" name="password" placeholder="Kodeord" />
        <input type="submit" value="login" />
    </form>
    <div class="mw300 center">
        <i>Hint: TannerHelland er adminadmin</i>
        <? if(isAuthed()): ?>
        <br>
        <a href="/admin">Administrator panel</a>
        <? endif; ?>
    </div>

    <?php 
        if(isset($_POST) && !empty($_POST)) {
            echo "Failed login attempt from user ".$_POST['username']." with password ".$_POST['password'];
        }
    ?>

<? require('footer.php'); ?>
