<?
    if(isset($_POST) && !empty($_POST) && strcmp($_POST['password'], $_POST['password_confirm']) == 0) {
        $db->query("INSERT INTO gamedb.user (name,password_hash,class_id, school_id)
                    VALUES (?,?,?,?);",
                    trim(ucfirst($_POST['fname'])." ".ucfirst($_POST['lname'])),
                    password_hash($_POST['password'], PASSWORD_DEFAULT),
                    $_POST['class_id'],
                    $_POST['school_id']
                  );

        $_SESSION['authorized'] = true;
        header("location: /");
    }
    elseif(isset($_POST) && !empty($_POST) && strcmp($_POST['password'], $_POST['password_confirm']) != 0) {
        echo "Du skal indtaste den samme kode 2 gange";
    }

    $db->query("SELECT 
                    class.id AS class_id, 
                    class.school_id AS school_id,
                    class.name AS class_name,
                    school.name AS school_name
                FROM
                    gamedb.class AS class
                LEFT JOIN 
                    gamedb.school AS school ON class.school_id = school.id
                WHERE class.key=?", $classKey);
    $classinfo = $db->fetch();
?>

<? require('header.php'); ?>
    <h1 class="textcenter"><?= $classinfo['school_name'] ?></h1>
    <h2 class="textcenter"><?= $classinfo['class_name'] ?></h2>
    <form action="" method="POST" class="mw300 center">
        <input type="hidden" name="class_id" value="<?= $classinfo['class_id'] ?>" />
        <input type="hidden" name="school_id" value="<?= $classinfo['school_id'] ?>" />
        <input type="text" name="fname" placeholder="Fornavn" />
        <input type="text" name="lname" placeholder="Efternavn" />
        <input type="password" name="password" placeholder="Kodeord" />
        <input type="password" name="password_confirm" placeholder="Gentag kodeord" />
        <input type="submit" value="Opret" />
    </form>

<? require('footer.php'); ?>
