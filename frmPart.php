<?php
// Start the session to access session variables
session_start();

// Database connection
$conn = new mysqli("localhost", "lee", "12345", "cs311a2024");
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Get the search query from the GET request, if it exists
$search = isset($_GET['search']) ? $_GET['search'] : '';

// Fetch PARTS type items with optional search query
$sql = "SELECT * FROM tblproduct WHERE type = 'PARTS'";

if ($search) {
    $search = $conn->real_escape_string($search);  // Prevent SQL injection
    $sql .= " AND name LIKE '%$search%'";
}

$result = $conn->query($sql);

// Function to find the correct image file with supported extensions
function findImage($name) {
    $extensions = ['jpg', 'jpeg', 'png'];
    foreach ($extensions as $ext) {
        $path = "../Parts/PARTS IMAGE/" . $name . "." . $ext;
        if (file_exists($path)) {
            return $path;
        }
    }
    return "../Parts/PARTS IMAGE/default.jpg"; // fallback image
}

// Function to get the profile image for the current user dynamically
function getProfileImage($username) {
    // Set the directory to the "PROFILE" folder
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
<html>
<head>
    <title>Parts</title>
    <style>
        body {
            font-family: 'Arial Rounded MT Bold', sans-serif;
            background-color: #d3f0ff;
            margin: 0;
            display: flex;
            flex-direction: column;
            align-items: center;
        }
        .navbar {
            background-color: #5f3dc4;
            padding: 20px;
            border-radius: 0 0 30px 30px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            width: 100%;
            color: white;
        }
        .navbar div {
            display: flex;
            gap: 20px;
        }
        .navbar strong {
            font-size: 24px;
        }
        .navbar span {
            font-size: 12px;
        }
        .navbar input[type="text"] {
            padding: 8px;
            border-radius: 20px;
            border: none;
            width: 250px;
            font-style: italic;
        }
        .navbar a {
            color: white;
            text-decoration: none;
            font-weight: bold;
            margin-right: 20px;
        }
        .navbar .user-area {
            display: flex;
            align-items: center;
            gap: 15px;
        }

        /* Smaller profile icon */
        .navbar .user-area img {
            width: 40px;
            height: 40px;
            object-fit: cover;
            border-radius: 50%;
        }

        .content {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 20px;
            width: 80%;
            margin: 20px 0;
        }
        .part-card {
            background-color: #89cff0;
            padding: 20px;
            border-radius: 20px;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 15px;
            width: 90%;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            height: 380px;  /* Adjusted to give space between content and button */
            position: relative;
        }
        .part-card img {
            width: 180px;
            height: 180px;
            object-fit: cover;
            border-radius: 15px;
            border: 3px solid #555;
        }
        .part-info {
            text-align: center;
        }
        .part-info h2 {
            margin: 0 0 15px 0;  /* More space between name and other elements */
            color: white;
        }
        .price {
            background-color: #ff7f3f;
            display: inline-block;
            padding: 12px 24px;  /* Increased padding for price */
            border-radius: 20px;
            font-weight: bold;
            font-size: 22px;  /* Larger price font */
            margin-top: 10px;
        }
        .stock {
            background-color: #4caf50;
            padding: 7px 18px;  /* Slightly larger padding for stock */
            border-radius: 20px;
            color: white;
            font-weight: bold;
        }
        .view-btn {
            margin-top: 15px;
            padding: 10px 20px;
            border-radius: 15px;
            border: 2px solid gray;
            background-color: white;
            font-weight: bold;
            cursor: pointer;
        }
        }
        .view-btn:hover {
            background-color: #eee;  /* Adds a hover effect */
        }

        /* Style for Clear Button */
        .clear-btn {
            background-color: #5f3dc4;
            color: white;
            padding: 10px 20px;
            font-weight: bold;
            border-radius: 30px;
            border: none;
            cursor: pointer;
            text-decoration: none;
            transition: background-color 0.3s;
            display: inline-block;
            margin-left: 10px;
        }

        .clear-btn:hover {
            background-color: #4b2a94;
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

      // Function to clear the search input
      function clearSearch() {
          document.getElementById('searchInput').value = '';
          window.location.href = 'frmPart.php'; // Redirect to the page to reset the search
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
    <?php while ($row = $result->fetch_assoc()): ?>
        <div class="part-card">
            <img src="<?php echo findImage($row['name']); ?>" alt="Part Image">
            <div class="part-info">
                <h2><?php echo htmlspecialchars($row['name']); ?></h2>
                <div class="stock">Stock: <?php echo $row['stock']; ?></div>
                <!-- Price below Stock -->
                <div class="price">₱<?php echo number_format($row['price'], 0); ?></div>
                <br>
                <!-- The View button now links to frmViewPart.php with the part's name as a query parameter -->
                <a href="frmViewPart.php?name=<?php echo urlencode($row['name']); ?>">
                    <button class="view-btn">View</button>
                </a>
            </div>
        </div>
    <?php endwhile; ?>
</div>

<?php $conn->close(); ?>
</body>
</html>
