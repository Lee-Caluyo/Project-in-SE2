<?php 
session_start();
if (!isset($_SESSION['username'])) {
    header("Location: frmLogin.php");
    exit();
}

$conn = new mysqli("localhost", "root", "", "cs311a2024");
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Fetch the contact number of the logged-in user from tbluser
$username = $_SESSION['username'];
$sql = "SELECT contact FROM tbluser WHERE username = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $username);
$stmt->execute();
$result = $stmt->get_result();

$user = $result->fetch_assoc();
$contactNumber = $user ? $user['contact'] : ''; // Set the contact number or empty if not found

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Add Repair Request</title>
    <style>
        body {
            font-family: 'Arial Rounded MT Bold', sans-serif;
            background-color: #f0f8ff;
            margin: 0;
            padding: 40px;
        }

        h2 {
            text-align: center;
            font-size: 32px;
            margin-bottom: 30px;
        }

        .repair-card {
            display: flex;
            gap: 30px;
            background: #89cff0;
            border-radius: 20px;
            padding: 30px;
            box-shadow: 0 0 10px gray;
            max-width: 1000px;
            margin: auto;
        }

        .repair-card img {
            width: 300px;
            height: 300px;
            object-fit: cover;
            border-radius: 15px;
        }

        .form-section {
            flex: 1;
            display: flex;
            flex-direction: column;
            gap: 15px;
        }

        .form-section label {
            font-weight: bold;
        }

        .form-section input,
        .form-section select,
        .form-section textarea {
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 14px;
            width: 100%;
        }

        .form-section textarea {
            resize: vertical;
        }

        .form-buttons {
            display: flex;
            gap: 20px;
            justify-content: flex-start;
        }

        .form-buttons .cancel-btn {
            margin-top: 20px;
            background-color: #dc3545;
            color: white;
            padding: 12px 20px;
            border: none;
            border-radius: 10px;
            font-size: 16px;
            cursor: pointer;
        }

        .form-buttons .submit-btn {
            margin-top: 20px;
            background-color: #28a745;
            color: white;
            padding: 12px 20px;
            border: none;
            border-radius: 10px;
            font-size: 16px;
            cursor: pointer;
            align-self: flex-start;
        }
    </style>
</head>
<body>

<h2>Add Repair Request</h2>

<div class="repair-card">
    <div class="form-section">
        <label for="concern">Concern</label>
        <textarea id="concern" name="concern" rows="3" placeholder="Enter concern"></textarea>

        <label for="branch">Branch</label>
        <select id="branch" name="branch" required>
            <option value="" disabled selected>Select a branch</option>
            <option value="ESTRADA STREET">ESTRADA STREET</option>
            <option value="ARELLANO EXTENSION STREET">ARELLANO EXTENSION STREET</option>
        </select>

        <label for="method1">Request Method</label>
        <select id="method1" name="method1">
            <option value="" disabled selected>Select Method</option>
            <option value="DROP OFF">Drop Off</option>
            <option value="PICK UP">Pick Up</option>
        </select>

        <label for="method2">Delivery Method</label>
        <select id="method2" name="method2">
            <option value="" disabled selected>Select Method</option>
            <option value="DELIVERY">Delivery</option>
            <option value="PICK UP">Pick Up</option>
        </select>

        <label for="contact">Contact Number</label>
        <input type="text" id="contact" name="contact" value="<?= htmlspecialchars($contactNumber) ?>" placeholder="09xxxxxxxxx" readonly>

        <label for="address">Address</label>
        <textarea id="address" name="address" rows="3" placeholder="Enter address"></textarea>

        <div class="form-buttons">
            <button class="cancel-btn" onclick="window.location.href='frmRepair.php';">Cancel</button>
            <button class="submit-btn">Submit Request</button>
        </div>
    </div>
</div>

</body>
</html>
