<?php
include 'db.php';

$error = "";
$json = "";
if (isset($_POST['fromUnity'])){
    if (isset($_POST['login']) && isset($_POST['password']) && isset($_POST['register'])){
        $ret =  $dbh->prepare("SELECT * FROM `users` WHERE `name` = {$dbh->quote($_POST['login'])}");
        $ret->execute();
        if ($ret->rowCount() == 0){
            $hash = password_hash($_POST['password'], PASSWORD_DEFAULT);
            $n = $dbh->query("INSERT INTO `users`(`name`, `password`) VALUES ('{$_POST['login']}', '{$hash}')");            
            $jsn = json_encode($dbh->query("SELECT * FROM `users` WHERE `id` = {$dbh->lastInsertId()}")->fetch(PDO::FETCH_ASSOC), JSON_UNESCAPED_UNICODE);
            $json = $jsn;
        }else{
            $error = "User exits";
        }
    }else
    if (isset($_POST['login']) && isset($_POST['password'])){
        $gen = $dbh->prepare("SELECT * FROM `users` WHERE `name` = {$dbh->quote($_POST['login'])}");
        $gen->execute();
        if ($gen->rowCount() > 0){
            $p = $gen->fetch(PDO::FETCH_ASSOC);
            if (password_verify($_POST['password'], $p['password'])){
                $json = json_encode($p, JSON_UNESCAPED_UNICODE);
            }else{
                $error = "Login or password error";
            }
        }else{
            $error = "Login or password error";
        }
    }else
    if (isset($_POST['save']) && isset($_POST['id'])){
      $upd = $dbh->prepare("UPDATE `users` SET `weapon`=".(int)$_POST['weapon'].",`corpus`=".(int)$_POST['corpus'].",`exp`=".(int)$_POST['exp']." WHERE `id` = ".(int)$_POST['id']);
      $upd->execute();
      $json = json_encode(array('error'=>'saved', "isError"=>"false"));
    }
    
}
if ($json != ""){
    echo $json;
}else{
    echo json_encode(array('error'=>$error, "isError"=>"true"));
}
?>
