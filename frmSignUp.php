<?php
session_start();
require_once "config.php";

$username = "";
$password = "";
$contact = "";
$error = "";
$success = "";

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["signup"])) {
    $username = trim($_POST["username"]);
    $password = trim($_POST["password"]);
    $contact = trim($_POST["contact"]);
    $dateCreated = date("m/d/Y");

    // Validation
    if (strlen($password) < 6) {
        $error = "Password must be at least 6 characters.";
    } else {
        // Check if username exists
        $checkSql = "SELECT * FROM tbluser WHERE username = ?";
        if ($stmt = mysqli_prepare($link, $checkSql)) {
            mysqli_stmt_bind_param($stmt, "s", $username);
            mysqli_stmt_execute($stmt);
            mysqli_stmt_store_result($stmt);
            if (mysqli_stmt_num_rows($stmt) > 0) {
                $error = "Username already exists.";
            } else {
                // Insert into tbluser
                $insertSql = "INSERT INTO tbluser (username, password, usertype, status, contact, DateCreated, CreatedBy)
                              VALUES (?, ?, 'CUSTOMER', 'ACTIVE', ?, ?, ?)";
                if ($stmtInsert = mysqli_prepare($link, $insertSql)) {
                    mysqli_stmt_bind_param($stmtInsert, "sssss", $username, $password, $contact, $dateCreated, $username);
                    if (mysqli_stmt_execute($stmtInsert)) {
                        // Insert into tbllogs
                        $logSql = "INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby)
                                   VALUES (?, ?, 'Add', 'Accounts Management', 'CUSTOMER', ?)";
                        $datelog = date("m/d/Y");
                        $timelog = date("h:i A");
                        if ($stmtLog = mysqli_prepare($link, $logSql)) {
                            mysqli_stmt_bind_param($stmtLog, "sss", $datelog, $timelog, $username);
                            mysqli_stmt_execute($stmtLog);
                            mysqli_stmt_close($stmtLog);
                        }
                        // Set success message
                        $success = "Account created successfully!";
                    } else {
                        $error = "Something went wrong while registering. Try again.";
                    }
                    mysqli_stmt_close($stmtInsert);
                }
            }
            mysqli_stmt_close($stmt);
        }
    }
}

mysqli_close($link);
?>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>Armando - Sign Up</title>
  <style>
    body {
      background-color: #cceaff;
      font-family: 'Arial Rounded MT Bold', sans-serif;
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100vh;
      margin: 0;
    }
    .container {
      text-align: center;
    }
    h1 {
      font-size: 5rem;
      color: #8e60d4;
      text-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
      margin-bottom: 0.3rem;
    }
    .subtitle {
      font-size: 1.5rem;
      letter-spacing: 2px;
      color: #000;
      margin-bottom: 2rem;
    }
    input[type="text"],
    input[type="password"] {
      width: 300px;
      padding: 15px;
      border: none;
      border-radius: 30px;
      margin: 10px 0;
      font-size: 1rem;
      background-color: #fff;
      box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);
    }
    .btn {
      padding: 15px 40px;
      margin: 10px;
      border: none;
      border-radius: 30px;
      font-size: 1.1rem;
      background-color: #6246d1;
      color: white;
      cursor: pointer;
      transition: background-color 0.3s ease;
    }
    .btn:hover {
      background-color: #5139af;
    }
    .login-text {
      margin-top: 10px;
      font-size: 0.95rem;
    }
    .login-text a {
      color: #6246d1;
      text-decoration: underline;
      cursor: pointer;
    }
    .message {
      color: red;
      margin-top: 10px;
    }
    .message.success {
      color: green;
    }
    /* Pop-up notification styles */
    #popup-notification {
      position: fixed;
      top: 0;
      left: 50%;
      transform: translateX(-50%);
      background-color: #4caf50;
      color: white;
      padding: 15px;
      font-size: 1.2rem;
      border-radius: 5px;
      display: none;
      z-index: 9999;
    }
    .popup-btn {
      background-color: #3e8e41;
      padding: 10px;
      border-radius: 5px;
      border: none;
      cursor: pointer;
      color: white;
      font-size: 1rem;
    }
  </style>
</head>
<body>
  <div class="container">
    <h1>Armando</h1>
    <div class="subtitle">BIKE SHOP, PARTS & SERVICES</div>

    <?php if (!empty($error)) : ?>
      <div class="message"><?php echo $error; ?></div>
    <?php endif; ?>

    <?php if (!empty($success)) : ?>
      <div id="popup-notification">
        <?php echo $success; ?>
        <br>
        <button class="popup-btn" onclick="redirectToLogin()">OK</button>
      </div>
      <script>
        // Show the pop-up notification
        document.getElementById('popup-notification').style.display = 'block';

        // Function to redirect to login page
        function redirectToLogin() {
          window.location.href = "frmLogin.php";
        }
      </script>
    <?php endif; ?>

    <form method="post" action="">
      <input type="text" name="username" placeholder="Username" value="<?php echo htmlspecialchars($username); ?>" required><br>
      <input type="password" name="password" placeholder="Password" value="<?php echo htmlspecialchars($password); ?>" required><br>
      <input type="text" name="contact" placeholder="Contact Number" value="<?php echo htmlspecialchars($contact); ?>" required><br>
      <button type="submit" name="signup" class="btn">Sign Up</button>
    </form>

    <div class="login-text">
      Already have an account? <a href="frmLogin.php">Login</a>
    </div>
  </div>
</body>
</html>
