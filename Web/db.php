<?php 
$connect = array(
    "host" => "localhost",
    "user" => "root",
    "pass" => "",
    "dbname" => "tod2_db"
);
$dbh = new PDO('mysql:host='.$connect['host'].';dbname='.$connect['dbname'], $connect['user'], $connect['pass']);
session_start();
?>