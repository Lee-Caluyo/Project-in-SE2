<?php
session_start();
if (!isset($_SESSION['username'])) {
    echo "error: not logged in";  // If user is not logged in
    exit();
}

$username = $_SESSION['username'];
$buyer = $_POST['buyer'];
$itemName = $_POST['name'];

$conn = new mysqli("localhost", "root", "", "cs311a2024");

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Debugging: check what values are being passed
error_log("Buyer: $buyer, Item Name: $itemName");

// SQL query to remove the item from the cart
$sql = "DELETE FROM tblcart WHERE buyer = ? AND name = ? AND status = 'IN CART'";

$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $buyer, $itemName);
$stmt->execute();

if ($stmt->affected_rows > 0) {
    echo "success";  // Item removed successfully
} else {
    echo "error: item not removed";  // Item not found or could not be removed
}

$stmt->close();
$conn->close();
?>
