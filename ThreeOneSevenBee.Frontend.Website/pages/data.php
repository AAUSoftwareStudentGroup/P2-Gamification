<?

requireAuth();

$userIds = array();
$data = array();


$db->query("SELECT user.id FROM user WHERE user.class_id=7");
while($row = $db->fetch())
    $userIds[] = $row['id'];

foreach ($userIds as $id) {
    $db->query("SELECT level.order AS level_id, user.name, user_level_progress.updated_at AS time_difference FROM gamedb.user_level_progress 
                LEFT JOIN level ON user_level_progress.level_id=level.id
                LEFT JOIN user ON user.id = user_level_progress.user_id
                WHERE user_id=? AND updated_at IS NOT NULL ORDER BY updated_at ASC;",
                $id
            );

    while($row = $db->fetch()) {
        if(!isset($data[$id])) {
            $data[$id] = array();
            $data[$id]['data'] = array();
            $data[$id]['name'] = $row['name'];
        }
        unset($row['name']);
        $data[$id]['data'][] = $row;
    }
}
$raw = $data;
$copy_data = $raw;


foreach ($data as $id => $userdata) {
    $data[$id]['total_time'] = $data[$id]['data'][count($data[$id]['data'])-1]['time_difference'] - $data[$id]['data'][0]['time_difference'];
    // $data[$id]['total_time'] = gmdate("H:i:s", $data[$id]['data'][count($data[$id]['data'])-1]['time_difference'] - $data[$id]['data'][0]['time_difference']);
    for($i = count($userdata['data'])-1; $i > 0; $i--) {
        $data[$id]['data'][$i]['time_difference'] -= $data[$id]['data'][$i-1]['time_difference'];
        // $data[$id]['data'][$i]['time_difference'] = gmdate("H:i:s", $data[$id]['data'][$i]['time_difference'] - $data[$id]['data'][$i-1]['time_difference']);
    }
    $data[$id]['data'][0]['time_difference'] = 0;
    // $data[$id]['data'][0]['time_difference'] = 'Nan';
    $data[$id]['levels_completed'] = count($data[$id]['data']);
}

foreach ($data as $user) {
    echo($user['total_time']."\n");
}
die();
die(json_encode($data, JSON_PRETTY_PRINT));


?>

<? require('header.php'); ?>

<style>
    table {
        /*float:left;*/
        width: 635px;
    }
</style>

<table>
    <tr>
        <th>Navn</th>
        <th>Total tid</th>
        <th>Levels startet</th>
    </tr>
    <? foreach ($data as $id => $user): ?>
    <tr>
        <td><?= $user['name'] ?></td>
        <td><?= $user['total_time'] ?></td>
        <td><?= $user['levels_completed'] ?></td>
    </tr>
    <? endforeach; ?>
</table>

<? foreach ($data as $id => $user): ?>
    <table>
        <tr>
            <th>Navn</th>
            <th>Level</th>
            <th>Tid brugt</th>
        </tr>
        <? foreach ($user['data'] as $entry): ?>
        <tr>
            <td><?= $user['name'] ?></td>
            <td><?= $entry['level_id'] ?></td>
            <td><?= $entry['time_difference'] ?></td>
        </tr>
        <? endforeach; ?>
    </table>
<? endforeach; ?>
