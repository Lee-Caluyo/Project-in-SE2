<?php
session_start();
require_once "config.php";

$username = "";
$password = "";
$error = "";

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['login'])) {
    $username = trim($_POST['username']);
    $password = trim($_POST['password']);

    $sql = "SELECT * FROM tbluser WHERE username = ?";
    if ($stmt = mysqli_prepare($link, $sql)) {
        mysqli_stmt_bind_param($stmt, "s", $username);
        mysqli_stmt_execute($stmt);
        $result = mysqli_stmt_get_result($stmt);

        if ($row = mysqli_fetch_assoc($result)) {
            if (strtoupper($row['status']) !== 'ACTIVE') {
                $error = "Account is inactive.";
            } else {
                // Assuming password verification (use password_verify() for hashed passwords)
                if ($row['password'] === $password) {
                    $_SESSION['username'] = $row['username'];
                    $_SESSION['usertype'] = $row['usertype'];
                    header("Location: /ict127-CS2A-2024/Customer/frmMain.php");
                    exit;
                } else {
                    $error = "Password is incorrect.";
                }
            }
        } else {
            $error = "Account doesn't exist.";
        }

        mysqli_stmt_close($stmt);
    }
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>Armando - Login</title>
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

    .input-container {
      position: relative;
      display: inline-block;
    }

    input[type="text"],
    input[type="password"] {
      width: 300px;
      padding: 15px 40px 15px 15px;
      border: none;
      border-radius: 30px;
      margin: 10px 0;
      font-size: 1rem;
      background-color: #fff;
      box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);
    }

    input::placeholder {
      color: #aaa;
    }

    .toggle-password {
      position: absolute;
      right: 15px;
      top: 50%;
      transform: translateY(-50%);
      cursor: pointer;
      font-size: 18px;
      color: #888;
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

    .signup-text {
      margin-top: 10px;
      font-size: 0.95rem;
    }

    .signup-text a {
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
  </style>
</head>
<body>
  <div class="container">
    <h1>Armando</h1>
    <div class="subtitle">BIKE SHOP, PARTS & SERVICES</div>

    <?php if (!empty($error)) : ?>
      <div class="message"><?php echo $error; ?></div>
    <?php endif; ?>

    <form method="post" action="">
      <input type="text" name="username" placeholder="Username" value="<?php echo htmlspecialchars($username); ?>" required><br>

      <div class="input-container">
        <input type="password" id="password" name="password" placeholder="Password" value="<?php echo htmlspecialchars($password); ?>" required>
        <span class="toggle-password" onclick="togglePassword()">&#128065;</span>
      </div><br>

      <button type="submit" name="login" class="btn">Login</button>
    </form>

    <div class="signup-text">
      Don't have an account? <a href="frmSignUp.php">Sign Up</a>
    </div>
  </div>

  <script>
    function togglePassword() {
      const pwField = document.getElementById('password');
      const icon = document.querySelector('.toggle-password');
      if (pwField.type === 'password') {
        pwField.type = 'text';
        icon.innerHTML = '&#128064;'; // open eye
      } else {
        pwField.type = 'password';
        icon.innerHTML = '&#128065;'; // closed eye
      }
    }
  </script>
</body>
</html>
