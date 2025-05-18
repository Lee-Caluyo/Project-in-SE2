	<?php

define('DB_SERVER', '127.0.0.1');
define('DB_USERNAME', 'lee');
define('DB_PASSWORD', '12345');
define('DB_NAME', 'cs311a2024');


$link = mysqli_connect(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);


if($link === false){
  
    die("Error: Could not connect. " . mysqli_connect_error());
    }
    date_default_timezone_set('Asia/Manila')

?>
