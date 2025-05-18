<?php
session_start();

if (!isset($_SESSION['username'])) {
    header("Location: frmLogin.php");
    exit; // This is important to stop the script after redirection
}
?>
