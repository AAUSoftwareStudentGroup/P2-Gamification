<?
    require('db.php');
    $db = new DB();

    session_start();

    if(isset($_POST) && !empty($_POST)) {
        $db->query("SELECT password_hash FROM user WHERE name = ?",
            $_POST['username']);
        
        if($row = $db->fetch()) {
            if(password_verify($_POST['password'], $row['password_hash'])) {
                $_SESSION['authorized'] = true;
                header("location: /");
            }
        }
    }
?>

<!DOCTYPE html>
<html>
<head>
    <title>Login</title>
</head>
<body>
    <form action="/login.php" method="POST">
        <input type="text" name="username" placeholder="Brugernavn" />
        <input type="password" name="password" placeholder="Kodeord" />
        <input type="submit" value="login" />
        <br>
        <i>Hint: TannerHelland er adminadmin</i>
    </form>
    <?php 
        if(isset($_POST) && !empty($_POST)) {
            echo "Failed login attempt from user ".$_POST['username']." with password ".$_POST['password'];
        }
    ?>
</body>
</html>