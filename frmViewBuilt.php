<?php
session_start();
if (!isset($_SESSION['username'])) {
    header("Location: frmLogin.php");
    exit;
}

if (!isset($_GET['bike_name']) || empty($_GET['bike_name'])) {
    echo "No bike selected!";
    exit;
}

$bike_name = $_GET['bike_name'];
$username  = $_SESSION['username'];
date_default_timezone_set('Asia/Manila');
$today = date("m/d/Y");

$conn = new mysqli("localhost", "lee", "12345", "cs311a2024");
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Fetch bike details
$sql = "SELECT * FROM tblproduct WHERE name = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $bike_name);
$stmt->execute();
$result = $stmt->get_result();
if (!$row = $result->fetch_assoc()) {
    echo "Bike not found!";
    exit;
}
$stmt->close();

// Fetch user contact
$userQuery = $conn->prepare("SELECT contact FROM tbluser WHERE username = ?");
$userQuery->bind_param("s", $username);
$userQuery->execute();
$userResult = $userQuery->get_result();
$userContact = ($userResult->num_rows > 0)
    ? $userResult->fetch_assoc()['contact']
    : '';
$userQuery->close();

// Check if already in this user's cart
$cartCheck = $conn->prepare("SELECT 1 FROM tblcart WHERE buyer = ? AND name = ?");
$cartCheck->bind_param("ss", $username, $bike_name);
$cartCheck->execute();
$cartRes = $cartCheck->get_result();
$isInCart = $cartRes->num_rows > 0;
$cartCheck->close();

// Handle Add‑to‑Cart submission
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['add_to_cart'])) {
    // 1) Mark product FOR PURCHASING
    $u = $conn->prepare("UPDATE tblproduct SET SaleStatus = 'FOR PURCHASING' WHERE name = ?");
    $u->bind_param("s", $bike_name);
    $u->execute();
    $u->close();

    // 2) Insert into tblcart
    $insert = $conn->prepare("
        INSERT INTO tblcart
          (buyer,name,specs,branch,price,type,contact,method2,status,quantity,code,DateAdded,DatePurchased)
        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)
    ");
    $method2       = "";
    $status        = "IN CART";
    $qty           = 1;
    $code          = "";
    $datePurchased = "";
    $insert->bind_param(
        "sssssssssssss",
        $username,
        $bike_name,
        $row['specs'],
        $row['branch'],
        $row['price'],
        $row['type'],
        $userContact,
        $method2,
        $status,
        $qty,
        $code,
        $today,
        $datePurchased
    );
    if ($insert->execute()) {
        header("Location: frmCart.php");
        exit;
    } else {
        echo "<script>alert('Failed to add to cart.');</script>";
    }
    $insert->close();
}

$conn->close();

function findImage($name) {
    foreach (['jpg','jpeg','png'] as $e) {
        $p = "../Built/BUILT IMAGE/{$name}.{$e}";
        if (file_exists($p)) return $p;
    }
    return "../Built/BUILT IMAGE/default.jpg";
}

function getProfileImage($username) {
    $dir = "/ict127-CS2A-2024/Profile/PROFILE";
    foreach (['jpg','jpeg','png'] as $e) {
        $p = $_SERVER['DOCUMENT_ROOT'] . "{$dir}/{$username}.{$e}";
        if (file_exists($p)) return "{$dir}/{$username}.{$e}";
    }
    return "data:image/svg+xml;utf8,"
      ."<svg xmlns='http://www.w3.org/2000/svg' width='100' height='100'>"
      ."<text x='50%' y='50%' text-anchor='middle' font-size='40' dy='.3em'>👤</text>"
      ."</svg>";
}

$profileImage = getProfileImage($username);
?>
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width,initial-scale=1.0">
  <title>Bike Details</title>
  <style>
    * { margin:0; padding:0; box-sizing:border-box; }
    body {
      font-family:'Arial Rounded MT Bold',sans-serif;
      background:#d3f0ff;
      min-height:100vh;
      display:flex;
      flex-direction:column;
    }
    .navbar {
      background:#5f3dc4; padding:20px;
      border-radius:0 0 30px 30px;
      display:flex; align-items:center; justify-content:space-between;
      color:white;
    }
    .navbar a { color:white; font-weight:bold; text-decoration:none; margin-right:20px; }
    .user-area { display:flex; align-items:center; gap:15px; }
    .user-area img { width:40px; height:40px; object-fit:cover; border-radius:50%; }

    .content {
      flex:1; display:flex; justify-content:center; align-items:center;
      padding:60px 0;
    }
    .bike-detail-card {
      background:#89cff0; padding:40px; width:90%; max-width:1200px;
      border-radius:30px; display:flex; gap:40px;
      box-shadow:0 0 15px rgba(0,0,0,0.1);
      position:relative;
    }
    .bike-detail-card img {
      width:300px; height:300px; object-fit:cover;
      border-radius:15px; border:3px solid #555;
    }
    .bike-info {
      flex:1; display:flex; flex-direction:column; justify-content:space-between;
    }
    .bike-info h2 { margin-bottom:10px; color:white; }
    .bike-info p { margin:5px 0; color:white; }
    .price {
      background:#ff7f3f; display:inline-block;
      padding:10px 20px; border-radius:20px;
      font-weight:bold; font-size:20px; margin-top:10px;
    }

    .add-to-cart-btn {
      margin-top:15px; padding:5px 15px; border-radius:15px;
      border:2px solid #1c9000; background:#fff; color:#1c9000;
      font-size:14px; font-weight:bold; cursor:pointer;
      position:absolute; bottom:30px; right:40px;
    }
    .add-to-cart-btn:hover { opacity:0.9; }
    .go-to-cart-btn {
      margin-top:15px; padding:5px 15px; border-radius:15px;
      border:2px solid #007bff; background:#fff; color:#007bff;
      font-size:14px; font-weight:bold; text-decoration:none;
      position:absolute; bottom:30px; right:120px;
    }
    .go-to-cart-btn:hover { opacity:0.9; }

    .back-btn {
      margin-top:15px; padding:5px 15px; border-radius:15px;
      border:2px solid #999; background:#fff; color:#333;
      font-size:14px; font-weight:bold; text-decoration:none;
      cursor:pointer; position:absolute; bottom:30px; right:260px;
    }
    .back-btn:hover { opacity:0.9; }

    .sale-status-box, .branch-box {
      display:inline-block; padding:10px 15px; margin:10px 0;
      font-weight:bold; font-size:14px; border-radius:0; color:black;
    }
    .sale-status-box { background:#ccc; border:2px solid #8c8c8c; }
    .branch-box      { background:#fff; border:2px solid #8c8c8c; }

    .modal {
      display:none; position:fixed; z-index:1; left:0; top:0;
      width:100%; height:100%; background:rgba(0,0,0,0.5); padding-top:60px;
    }
    .modal-content {
      background:#fefefe; margin:5% auto; padding:20px;
      border:1px solid #888; width:80%; max-width:400px; border-radius:10px;
    }
    .modal-footer { text-align:center; }
    .modal-footer button {
      padding:10px 20px; border:none; font-size:16px; cursor:pointer;
    }
    .modal-footer .cancel   { background:#f44336; color:white; }
    .modal-footer button:not(.cancel) { background:#4CAF50; color:white; }
    .modal-footer button:hover { opacity:0.8; }
  </style>
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
      <a href="logout.php">Logout</a>
      <div class="cart">
        <a href="frmCart.php" title="Go to Cart">
          <img src="Icons/cart_icon.png" alt="Cart">
        </a>
      </div>
      <a href="frmProfile.php">
        <img src="<?= $profileImage ?>" alt="Profile Icon">
      </a>
    </div>
  </div>

  <div class="content">
    <div class="bike-detail-card">
      <img src="<?= findImage($bike_name) ?>" alt="Bike">
      <div class="bike-info">
        <div>
          <h2><?= htmlspecialchars($row['name']) ?></h2>
          <p><strong>Specifications:</strong><br><?= nl2br(htmlspecialchars($row['specs'])) ?></p>
          <div class="sale-status-box">
            Sale Status: <?= htmlspecialchars($row['SaleStatus']) ?>
          </div>
          <div class="branch-box">
            Branch: <?= htmlspecialchars($row['branch']) ?>
          </div>
          <div class="price">₱<?= number_format($row['price'],0) ?></div>
        </div>

        <?php if ($isInCart): ?>
          <a href="frmCart.php"  class="go-to-cart-btn">View Cart</a>
          <a href="frmBuilt.php" class="back-btn">Back</a>

        <?php elseif ($row['SaleStatus'] !== 'SOLD' && $row['SaleStatus'] !== 'FOR PURCHASING'): ?>
          <button class="add-to-cart-btn" id="addToCartBtn">Add to Cart</button>
          <a href="frmBuilt.php" class="back-btn">Back</a>

        <?php else: /* FOR PURCHASING or SOLD */ ?>
          <a href="frmBuilt.php" class="back-btn">Back</a>
        <?php endif; ?>

      </div>
    </div>
  </div>

  <!-- confirmation modal -->
  <div id="confirmationModal" class="modal">
    <div class="modal-content">
      <h3>Are you sure you want to add this to your cart?</h3>
      <div class="modal-footer">
        <button class="cancel" id="cancelBtn">No</button>
        <button id="confirmBtn">Yes</button>
      </div>
    </div>
  </div>

  <script>
    var modal      = document.getElementById("confirmationModal"),
        addBtn     = document.getElementById("addToCartBtn"),
        cancelBtn  = document.getElementById("cancelBtn"),
        confirmBtn = document.getElementById("confirmBtn");

    if (addBtn) {
      addBtn.onclick     = ()=> modal.style.display = "block";
      cancelBtn.onclick  = ()=> modal.style.display = "none";
      confirmBtn.onclick = ()=> {
        var f=document.createElement("form");
        f.method="POST";
        var i=document.createElement("input");
        i.type="hidden"; i.name="add_to_cart"; i.value="1";
        f.appendChild(i);
        document.body.appendChild(f);
        f.submit();
      };
    }
  </script>
</body>
</html>
