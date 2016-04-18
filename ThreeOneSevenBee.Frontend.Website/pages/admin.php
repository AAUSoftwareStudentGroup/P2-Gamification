<?
requireAuth();

if(isset($_POST) && !empty($_POST)) {
    switch ($_POST['action']) {
        case 'create_school':
            $db->query("INSERT INTO gamedb.school (name) VALUES (?)",
                        $_POST['school_name']);
            break;
        case 'create_class':
            if($_POST['school_id'] > 0) {
                $db->query("INSERT INTO gamedb.class (class.school_id, class.name, class.key) VALUES (?, ?, ?)",
                            $_POST['school_id'],
                            $_POST['class_name'],
                            getRandomString(6)
                            );
            }
            break;
        case 'create_level_category':
            $db->query("INSERT INTO gamedb.level_category (level_category.name) VALUES (?)",
                        $_POST['cat_name']
                    );
            break;
        case 'create_level':
            $db->query("INSERT INTO gamedb.level 
                            (level.level_category_id, level.initial_expression, level.star_expressions)
                        VALUES (?, ?, ?)",
                        $_POST['category_id'],
                        $_POST['level_expression'],
                        $_POST['star_expressions']
                    );
            break;
    }
}




//// PREPARE
$users = array();
$supervisors = array();
$classes = array();
$schools = array();
$categories = array();
$levels = array();

$db->query("SELECT 
                user.id AS id,
                user.name AS name,
                user.privelige_id AS privilege,
                class.name AS class_name,
                school.name AS school_name
            FROM
                gamedb.user AS user
            LEFT JOIN
                gamedb.class AS class ON user.class_id = class.id
            LEFT JOIN
                gamedb.school AS school ON user.school_id = school.id;"
        );
while($row = $db->fetch())
    $users[] = $row;

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

///// CLASSES
$db->query( "SELECT 
                class.id AS class_id,
                class.key AS 'key',
                class.name AS class_name, 
                school.name AS school_name
            FROM gamedb.class AS class
            LEFT JOIN gamedb.school AS school
                ON class.school_id = school.id
            WHERE class.deleted_at IS NULL;"
            );
while($row = $db->fetch())
    $classes[] = $row;

///// SCHOOLS
$db->query( "SELECT 
                school.id,
                school.name
            FROM gamedb.school AS school
            WHERE school.deleted_at IS NULL;"
            );
while($row = $db->fetch())
    $schools[] = $row;


///// CATEGORIES
$db->query( "SELECT 
                c.id,
                c.name
            FROM gamedb.level_category AS c"
            );
while($row = $db->fetch())
    $categories[] = $row;

///// LEVELS
$db->query("SELECT 
                l.id AS level_id, 
                c.id AS category_id, 
                c.name AS category_name, 
                l.initial_expression, 
                l.star_expressions 
            FROM gamedb.level AS l
            LEFT JOIN gamedb.level_category AS c
            ON l.level_category_id = c.id;"
        );
while($row = $db->fetch())
    $levels[] = $row;


?>

<? require('header.php'); ?>

<div class="row">
    <h2>Users</h2>
    <table class="r2_4 user_table">
        <tr>
            <th colspan="1">user_id</th>
            <th colspan="3">user_name</th>
            <th colspan="3">privilege</th>
            <th colspan="3">class_name</th>
            <th colspan="3">school_name</th>
            <th colspan="1">del</th>
        </tr>
        <? foreach ($users as $user): ?>
        <tr>
            <td colspan="1"><?= $user['id'] ?></td>
            <td colspan="3"><?= $user['name'] ?></td>
            <td colspan="3"><?= ($user['privilege'] == 1 ? "student" : ($user['privilege'] == 2 ? "supervisor" : "admin")) ?></td>
            <td colspan="3"><?= $user['class_name'] ?></td>
            <td colspan="3"><?= $user['school_name'] ?></td>
            <td colspan="1" class="delete" data-id="<?= $user['id'] ?>">x</td>
        </tr>
        <? endforeach; ?>
    </table>
    <form class="r2_4 mw300" method="POST" action="">
        <h2>Create user</h2>
        <input type="hidden" name="action" value="create_user" />
        <input type="text" name="class_name" placeholder="User name" />
        <select name="class_id">
            <option disabled selected value="-1">Vælg klasse</option>
            <? foreach($schools as $school): ?>
            <option value="<?= $school['id'] ?>"><?= $school['name'] ?></option>
            <? endforeach; ?>
        </select>
        <input type="submit" value="Create" />
    </form>
</div>

<div class="row">
    <h2>Supervisors</h2>
    <table class="r2_4 supervisor_table">
        <tr>
            <th colspan="1">user_id</th>
            <th colspan="3">user_name</th>
            <th colspan="5">email</th>
            <th colspan="3">class_name</th>
            <th colspan="3">school_name</th>
            <!-- <th colspan="1">del</th> -->
        </tr>
        <? foreach ($supervisors as $sup): ?>
        <tr>
            <td colspan="1"><?= $sup['user_id'] ?></td>
            <td colspan="3"><?= $sup['user_name'] ?></td>
            <td colspan="5"><?= $sup['email'] ?></td>
            <td colspan="3"><?= $sup['class_name'] ?></td>
            <td colspan="3"><?= $sup['school_name'] ?></td>
            <!-- <td colspan="1" class="delete">x</td> -->
        </tr>
        <? endforeach; ?>
    </table>
</div>

<div class="row">
    <div class="r2_4">
        <h2>Classes</h2>
        <table class="r1_1 class_table">
            <tr>
                <th colspan="1">class_id</th>
                <th colspan="2">key</th>
                <th colspan="2">class_name</th>
                <th colspan="4">school_name</th>
                <th colspan="1">del</th>
            </tr>
            <? foreach ($classes as $class): ?>
            <tr>
                <td colspan="1"><?= $class['class_id'] ?></td>
                <td colspan="2"><?= $class['key'] ?></td>
                <td colspan="2"><?= $class['class_name'] ?></td>
                <td colspan="4"><?= $class['school_name'] ?></td>
                <td colspan="1" class="delete" data-id="<?= $class['class_id'] ?>">x</td>
            </tr>
            <? endforeach; ?>
        </table>
    </div>
    <form class="r2_4 mw300" method="POST" action="">
        <h2>Create class</h2>
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

<div class="row">
    <div class="r2_4">
        <h2>Schools</h2>
        <table class="r1_1 school_table">
            <tr>
                <th colspan="1">school_id</th>
                <th colspan="5">school_name</th>
                <th colspan="1">del</th>
            </tr>
            <? foreach ($schools as $school): ?>
            <tr>
                <td colspan="1"><?= $school['id'] ?></td>
                <td colspan="5"><?= $school['name'] ?></td>
                <td colspan="1" class="delete" data-id="<?= $school['id'] ?>">x</td>
            </tr>
            <? endforeach; ?>
        </table>
    </div>
    <form class="r2_4 mw300" method="POST" action="">
        <h2>Create school</h2>
        <input type="hidden" name="action" value="create_school" />
        <input type="text" name="school_name" placeholder="school name" />
        <input type="submit" value="Create" />
    </form>
</div>
<div class="row">
    <br>
    <hr />
    <br>
    <div class="r2_4">
        <h2>Classes</h2>
        <table class="r1_1 category_table">
            <tr>
                <th colspan="1">Cat_id</th>
                <th colspan="5">Category</th>
                <th colspan="1">del</th>
            </tr>
            <? foreach ($categories as $cat): ?>
            <tr>
                <td colspan="1"><?= $cat['id'] ?></td>
                <td colspan="5"><?= $cat['name'] ?></td>
                <td colspan="1" class="delete" data-id="<?= $cat['id'] ?>">x</td>
            </tr>
            <? endforeach; ?>
        </table>
    </div>
    <div class="r1_4">
        <form method="POST" action="">
            <h2>Create Level category</h2>
            <input type="hidden" name="action" value="create_level_category" />
            <input type="text" name="cat_name" placeholder="Level Category name" />
            <input type="submit" value="Create" />
        </form>
    </div>
</div>
<div class="row">
    <div class="r2_4">
        <h2>Classes</h2>
        <table class="r1_1 level_table">
            <tr>
                <th colspan="1">Cat_id</th>
                <th colspan="1">lvl_id</th>
                <th colspan="2">Category</th>
                <th colspan="3">initial_expression</th>
                <th colspan="4">Star_expressions</th>
                <th colspan="1">del</th>
            </tr>
            <? foreach ($levels as $lvl): ?>
            <tr>
                <td colspan="1"><?= $lvl['category_id'] ?></td>
                <td colspan="1"><?= $lvl['level_id'] ?></td>
                <td colspan="2"><?= $lvl['category_name'] ?></td>
                <td colspan="3"><?= $lvl['initial_expression'] ?></td>
                <td colspan="4"><?= $lvl['star_expressions'] ?></td>
                <td colspan="1" class="delete" data-id="<?= $lvl['level_id'] ?>">x</td>
            </tr>
            <? endforeach; ?>
        </table>
    </div>
    <div class="r1_4">
        <form method="POST" action="" class="level_table">
            <h2>Create Level</h2>
            <input type="hidden" name="action" value="create_level" />
            <select name="category_id">
                <? foreach ($categories as $cat): ?>
                <option value="<?= $cat['id'] ?>"><?= $cat['name'] ?></option>
                <? endforeach; ?>
            </select>
            <input type="text" name="level_expression" placeholder="Level expression" />
            <input type="text" name="star_expressions" placeholder="Level starExpressions '|' seperated" />
            <input type="submit" value="Create" />
        </form>
    </div>
</div>

<script>
    $('.class_table').on('click', '.delete', function() {
        var id = $(this).data('id');
        $.get( "/api", { action: "delete_class_by_id", class_id: id });
        $(this).parent().remove();
    });
    $('.school_table').on('click', '.delete', function() {
        var id = $(this).data('id');
        $.get( "/api", { action: "delete_school_by_id", school_id: id });
        $(this).parent().remove();
    });
    $('.category_table').on('click', '.delete', function() {
        var id = $(this).data('id');
        $.get( "/api", { action: "delete_category_by_id", category_id: id });
        window.location = window.location;
    });
    $('.level_table').on('click', '.delete', function() {
        var id = $(this).data('id');
        $.get( "/api", { action: "delete_level_by_id", level_id: id });
        $(this).parent().remove();
    });
</script>

<? require('footer.php'); ?>