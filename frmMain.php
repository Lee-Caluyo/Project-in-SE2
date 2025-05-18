<?php
session_start();

if (!isset($_SESSION['username'])) {
    echo "<!DOCTYPE html>
    <html>
    <head>
        <title>Access Denied</title>
        <style>
            body {
                background-color: #ffe6e6;
                font-family: 'Arial Rounded MT Bold', sans-serif;
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: center;
                height: 100vh;
                margin: 0;
            }
            .error-box {
                background-color: #ffcccc;
                padding: 30px 50px;
                border-radius: 20px;
                box-shadow: 0 0 10px rgba(0,0,0,0.2);
                text-align: center;
            }
            h1 {
                color: #cc0000;
            }
            a {
                display: inline-block;
                margin-top: 20px;
                padding: 10px 25px;
                background-color: #cc0000;
                color: white;
                text-decoration: none;
                border-radius: 30px;
                font-weight: bold;
            }
            a:hover {
                background-color: #990000;
            }
        </style>
    </head>
    <body>
        <div class='error-box'>
            <h1>Access Denied</h1>
            <p>You cannot access this page without logging in.</p>
            <a href='frmLogin.php'>Go to Login</a>
        </div>
    </body>
    </html>";
    exit;
}

$conn = new mysqli("localhost", "lee", "12345", "cs311a2024");

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Function to get the profile image for the current user dynamically
function getProfileImage($username) {
    $profileDir = "/ict127-CS2A-2024/Profile/PROFILE"; // Relative path from the root of the server
    $extensions = ['jpg', 'jpeg', 'png'];
    
    // Try each extension based on the username
    foreach ($extensions as $ext) {
        $profileImagePath = $_SERVER['DOCUMENT_ROOT'] . $profileDir . '/' . $username . '.' . $ext;
        if (file_exists($profileImagePath)) {
            return $profileDir . '/' . $username . '.' . $ext; // Return relative path
        }
    }
    
    // Return the default icon if no profile image found (use icon 👤 here)
    return "data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='100' height='100'><text x='50%' y='50%' text-anchor='middle' font-size='40' dy='.3em'>👤</text></svg>";
}

// Get the current username from the session
$username = isset($_SESSION['username']) ? $_SESSION['username'] : 'guest'; // Default to 'guest' if not logged in
$profileImage = getProfileImage($username); // Get the profile image for the current user
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Main</title>
    <style>
        body {
            font-family: 'Arial Rounded MT Bold', sans-serif;
            background-color: #d3f0ff;
            margin: 0;
        }
        .navbar {
            background-color: #5f3dc4;
            padding: 20px;
            border-radius: 0 0 30px 30px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            color: white;
        }
        .navbar a {
            color: white;
            font-weight: bold;
            text-decoration: none;
            margin-right: 20px;
        }
        .navbar .user-area {
            display: flex;
            align-items: center;
            gap: 15px;
        }
        .navbar .user-area img {
            width: 40px; /* Adjusted size of profile image */
            height: 40px; /* Adjusted size of profile image */
            object-fit: cover;
            border-radius: 50%; /* Circular profile image */
        }
        .content {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-top: 20px;
        }
    </style>
    <script>
      function confirmLogout(event) {
          if (confirm("Are you sure you want to logout?")) {
              window.location.href = "frmLogin.php";
          } else {
              event.preventDefault(); // Prevent default action (redirecting) on "Cancel"
          }
      }
    </script>
</head>
<body>

<div class="navbar">
    <div>
        <strong>Armando</strong><br>
        <span>BIKE SHOP, PARTS & SERVICES</span>
    </div>
    <div class="user-area">
        <a href="frmBuilt.php">Built</a>
        <a href="frmRepair.php">Repair</a>
        <a href="frmPart.php">Parts</a>
        <a href="frmLogin.php">Logout</a>

        <div class="cart">
            <a href="frmCart.php" title="Go to Cart">
                <!-- Corrected the image source path -->
		<img src="http://localhost/ict127-CS2A-2024/Customer/Icons/cart_icon.png" alt="Cart">
            </a>
        </div>

        <a href="frmProfile.php">
            <img src="<?php echo $profileImage; ?>" alt="Profile Icon">
        </a>
    </div>
</div>


<div class="content">
    <h2>Welcome to the Bike Shop</h2>
    <p>Explore our Built, Repair, and Parts categories.</p>
</div>

</body>
</html>

<?php 
$conn->close(); 
?>
