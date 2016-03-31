<?
requireAuth();

if(isset($_POST) && !empty($_POST)) {
    // die(var_dump($_POST));
    switch ($_POST['action']) {
        case 'create_school':
            $db->query("INSERT INTO gamedb.school (name) VALUES (?)",
                        $_POST['school_name']);
            break;
        case 'create_class':
            // echo getRandomString(6);
            // die(var_dump($_POST));
            if($_POST['school_id'] > 0) {
                $db->query("INSERT INTO gamedb.class (class.school_id, class.name, class.key) VALUES (?, ?, ?)",
                            $_POST['school_id'],
                            $_POST['class_name'],
                            getRandomString(6)
                            );
            }
            break;
    }
}




//// PREPARE
$supervisors = array();
$schools = array();
$classes = array();

///// SUPERVISORS
$db->query( "SELECT 
                user.id AS user_id, 
                class.id AS class_id, 
                school.id AS school_id, 
                user.name AS user_name, 
                sup.email AS email, 
                class.name AS class_name, 
                school.name AS school_name 
            FROM gamedb.supervisor AS sup
            LEFT JOIN gamedb.user AS user 
                ON sup.user_id = user.id
            LEFT JOIN gamedb.class AS class 
                ON sup.user_class_id = class.id
            LEFT JOIN gamedb.school AS school 
                ON sup.user_school_id = school.id;"
            );
while($row = $db->fetch())
    $supervisors[] = $row;

///// SCHOOLS
$db->query( "SELECT 
                school.id,
                school.name
            FROM gamedb.school AS school;"
            );
while($row = $db->fetch())
    $schools[] = $row;

///// CLASSES
$db->query( "SELECT 
                class.id AS class_id,
                class.key AS 'key',
                class.name AS class_name, 
                school.name AS school_name
            FROM gamedb.class AS class
            LEFT JOIN gamedb.school AS school
                ON class.school_id = school.id;"
            );
while($row = $db->fetch())
    $classes[] = $row;



?>

<? require('header.php'); ?>

<div class="row">
    <h2>Supervisors</h2>
    <table class="r2_4">
        <tr>
            <th>user_id</th>
            <th>class_id</th>
            <th>school_id</th>
            <th>user_name</th>
            <th>email</th>
            <th>class_name</th>
            <th>school_name</th>
        </tr>
        <? foreach ($supervisors as $sup): ?>
        <tr>
            <td><?= $sup['user_id'] ?></td>
            <td><?= $sup['class_id'] ?></td>
            <td><?= $sup['school_id'] ?></td>
            <td><?= $sup['user_name'] ?></td>
            <td><?= $sup['email'] ?></td>
            <td><?= $sup['class_name'] ?></td>
            <td><?= $sup['school_name'] ?></td>
        </tr>
        <? endforeach; ?>
    </table>
</div>

<div class="row">
    <div class="r2_4">
        <h2>Schools</h2>
        <table class="r1_1">
            <tr>
                <th>school_id</th>
                <th>school_name</th>
            </tr>
            <? foreach ($schools as $school): ?>
            <tr>
                <td><?= $school['id'] ?></td>
                <td><?= $school['name'] ?></td>
            </tr>
            <? endforeach; ?>
        </table>
    </div>
    <form class="r2_4 mw300" method="POST" action="">
        <h2>create school</h2>
        <input type="hidden" name="action" value="create_school" />
        <input type="text" name="school_name" placeholder="school name" />
        <input type="submit" value="Create" />
    </form>
</div>

<div class="row">
    <div class="r2_4">
        <h2>Classes</h2>
        <table class="r1_1">
            <tr>
                <th>class_id</th>
                <th>key</th>
                <th>class_name</th>
                <th>school_name</th>
            </tr>
            <? foreach ($classes as $class): ?>
            <tr>
                <td><?= $class['class_id'] ?></td>
                <td><?= $class['key'] ?></td>
                <td><?= $class['class_name'] ?></td>
                <td><?= $class['school_name'] ?></td>
            </tr>
            <? endforeach; ?>
        </table>
    </div>
    <form class="r2_4 mw300" method="POST" action="">
        <h2>create class</h2>
        <input type="hidden" name="action" value="create_class" />
        <input type="text" name="class_name" placeholder="class name" />
        <select name="school_id">
            <option disabled selected value="-1">Vælg skole</option>
            <? foreach($schools as $school): ?>
            <option value="<?= $school['id'] ?>"><?= $school['name'] ?></option>
            <? endforeach; ?>
        </select>
        <input type="submit" value="Create" />
    </form>
</div>

<? require('footer.php'); ?>