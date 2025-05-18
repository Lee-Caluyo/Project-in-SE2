<?php
session_start();
if (!isset($_SESSION['username'])) {
    header("Location: frmLogin.php");
    exit;
}

$conn = new mysqli("localhost", "lee", "12345", "cs311a2024");
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Handle search functionality
$searchQuery = "";
if (isset($_GET['search']) && !empty($_GET['search'])) {
    $searchQuery = $_GET['search'];
}

// Modify the SQL query to filter by type and optionally by name (based on the search query)
$sql = "SELECT * FROM tblproduct WHERE type = 'BUILT'";

if ($searchQuery) {
    $sql .= " AND name LIKE '%" . $conn->real_escape_string($searchQuery) . "%'";
}

$result = $conn->query($sql);

// Function to find the correct image file for built bikes
function findImage($name) {
    $extensions = ['jpg', 'jpeg', 'png'];
    foreach ($extensions as $ext) {
        $path = "../Built/BUILT IMAGE/" . $name . "." . $ext;
        if (file_exists($path)) return $path;
    }
    return "../Built/BUILT IMAGE/default.jpg";
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
<html>
<head>
    <title>Built Bikes</title>
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
        .navbar input[type="text"] {
            padding: 10px;
            border-radius: 20px;
            border: none;
            width: 300px;
            font-style: italic;
        }
        .navbar a {
            color: white;
            font-weight: bold;
            text-decoration: none;
            margin-right: 20px;
        }
        .user-area {
            display: flex;
            align-items: center;
            gap: 15px;
        }
        .user-area img {
            width: 40px; /* Adjusted size of profile image */
            height: 40px; /* Adjusted size of profile image */
            object-fit: cover;
            border-radius: 50%; /* Circular profile image */
        }
        .bike-card {
            background-color: #89cff0;
            margin: 30px auto;
            padding: 20px;
            width: 80%;
            border-radius: 30px;
            display: flex;
            align-items: center;
            gap: 40px;
        }
        .bike-card img {
            width: 150px; /* Adjusted to match frmPart image size */
            height: 150px; /* Adjusted to match frmPart image size */
            object-fit: cover;
            border-radius: 15px;
            border: 3px solid #555;
        }
        .bike-info {
            flex: 1;
        }
        .bike-info h2 {
            margin: 0 0 10px 0;
            color: white;
        }
        .price {
            background-color: #ff7f3f;
            display: inline-block;
            padding: 10px 20px;
            border-radius: 20px;
            font-weight: bold;
            font-size: 20px;
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
        .view-btn:hover {
            background-color: #eee;
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
          window.location.href = 'frmBuilt.php'; // Redirect to the page to reset the search
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

<?php while ($row = $result->fetch_assoc()): ?>
    <div class="bike-card">
        <img src="<?php echo findImage($row['name']); ?>" alt="Bike Image">
        <div class="bike-info">
            <h2><?php echo htmlspecialchars($row['name']); ?></h2>
            <div class="price">₱<?php echo number_format($row['price'], 0); ?></div>
            <br>
            <!-- Update the "View" button to link to frmViewBuilt.php -->
            <a href="frmViewBuilt.php?bike_name=<?php echo urlencode($row['name']); ?>">
                <button class="view-btn">View</button>
            </a>
        </div>
    </div>
<?php endwhile; ?>


<?php $conn->close(); ?>
</body>
</html>
