<?php
session_start();

$conn = new mysqli("localhost", "lee", "12345", "cs311a2024");
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$productName = isset($_GET['name']) ? $_GET['name'] : '';
$sql = "SELECT * FROM tblproduct WHERE name = '$productName' AND type = 'PARTS'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
} else {
    die("Product not found.");
}

function findImage($name) {
    $extensions = ['jpg', 'jpeg', 'png'];
    foreach ($extensions as $ext) {
        $path = "../Parts/PARTS IMAGE/" . $name . "." . $ext;
        if (file_exists($path)) return $path;
    }
    return "../Parts/PARTS IMAGE/default.jpg";
}

function getProfileImage($username) {
    $profileDir = "/ict127-CS2A-2024/Profile/PROFILE";
    $extensions = ['jpg', 'jpeg', 'png'];
    foreach ($extensions as $ext) {
        $path = $_SERVER['DOCUMENT_ROOT'] . $profileDir . '/' . $username . '.' . $ext;
        if (file_exists($path)) return $profileDir . '/' . $username . '.' . $ext;
    }
    return "data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='100' height='100'><text x='50%' y='50%' text-anchor='middle' font-size='40' dy='.3em'>👤</text></svg>";
}

// Handle Add to Cart
if (isset($_POST['addToCart'])) {
    $buyer = $_SESSION['username'];
    $branch = $_POST['branch'];
    $quantity = (int) $_POST['quantity'];
    $name = $row['name'];
    $specs = ''; // blank
    $price = $row['price'];
    $type = $row['type'];
    $method2 = ''; // blank
    $status = "IN CART";
    $code = ''; // blank
    $dateAdded = date('Y-m-d');

    // Get buyer contact from tbluser
    $getContact = "SELECT contact FROM tbluser WHERE username = '$buyer'";
    $contactResult = $conn->query($getContact);
    $contact = '';
    if ($contactResult && $contactResult->num_rows > 0) {
        $contact = $contactResult->fetch_assoc()['contact'];
    }

    // Check if there is enough stock available
    if ($quantity <= $row['stock']) {
        // Insert into tblcart
        $insert = "INSERT INTO tblcart (buyer, name, specs, branch, price, type, contact, method2, status, quantity, code, DateAdded, DatePurchased)
                   VALUES ('$buyer', '$name', '$specs', '$branch', '$price', '$type', '$contact', '$method2', '$status', '$quantity', '$code', '$dateAdded', '')";
        $conn->query($insert);

        // Update stock in tblproduct
        $newStock = $row['stock'] - $quantity;
        $updateStock = "UPDATE tblproduct SET stock = $newStock WHERE name = '$name' AND type = 'PARTS'";
        $conn->query($updateStock);

        echo "<script>alert('Added to cart successfully.'); window.location.href='frmPart.php';</script>";
    } else {
        // If not enough stock, show error message
        echo "<script>alert('Not enough stock available.'); window.location.href='frmPart.php';</script>";
    }

    exit();
}

$username = isset($_SESSION['username']) ? $_SESSION['username'] : 'guest';
$profileImage = getProfileImage($username);
?>

<!DOCTYPE html>
<html>
<head>
    <title>View Part</title>
    <style>
        /* Same CSS as before (navbar, layout, etc.) */
        body {
            font-family: 'Arial Rounded MT Bold', sans-serif;
            background-color: #d3f0ff;
        }
        .navbar {
            background-color: #5f3dc4;
            padding: 20px;
            border-radius: 0 0 30px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            color: white;
        }
        .navbar a {
            color: white;
            margin-right: 20px;
            font-weight: bold;
    	    text-decoration: none; /* This removes the underline */
        }
        .user-area {
            display: flex;
            align-items: center;
            gap: 15px;
        }
        .user-area img {
            width: 40px; height: 40px; border-radius: 50%;
        }
        .content {
            padding: 50px;
            display: flex;
            justify-content: center;
        }
        .part-card {
            display: flex;
            gap: 30px;
            background: #89cff0;
            border-radius: 20px;
            padding: 30px;
            box-shadow: 0 0 10px gray;
        }
        .part-card img {
            width: 300px;
            height: 300px;
            object-fit: cover;
            border-radius: 15px;
        }
        .price {
            background-color: orange;
            padding: 10px 20px;
            border-radius: 10px;
            font-weight: bold;
            font-size: 20px;
        }
        .add-to-cart-btn {
            padding: 10px 20px;
            background: limegreen;
            border: none;
            border-radius: 10px;
            font-weight: bold;
            cursor: pointer;
            margin-top: 20px;
        }

        /* Modal Styles */
        .modal-overlay {
            position: fixed; top: 0; left: 0;
            width: 100%; height: 100%;
            background-color: rgba(0,0,0,0.5);
            display: none;
            align-items: center;
            justify-content: center;
        }
        .modal {
            background: white;
            padding: 30px;
            border-radius: 15px;
            width: 400px;
        }
        .modal input, .modal select {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
        }
        .modal button {
            padding: 10px 20px;
            border-radius: 10px;
            font-weight: bold;
            cursor: pointer;
        }
        .modal .cancel-btn { background-color: gray; color: white; }
        .modal .add-btn { background-color: green; color: white; }
        .error-msg { color: red; font-size: 14px; }
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
        <a href="frmLogin.php">Logout</a>
        <a href="frmCart.php"><img src="http://localhost/ict127-CS2A-2024/Customer/Icons/cart_icon.png" alt="Cart"></a>
        <a href="frmProfile.php"><img src="<?php echo $profileImage; ?>" alt="Profile Icon"></a>
    </div>
</div>

<div class="content">
    <div class="part-card">
        <img src="<?php echo findImage($row['name']); ?>" alt="Part Image">
        <div>
            <h2><?php echo htmlspecialchars($row['name']); ?></h2>
            <p>Branch: <?php echo $row['branch']; ?></p>
            <p>Stock: <?php echo $row['stock']; ?></p>
            <div class="price">₱<?php echo number_format($row['price'], 0); ?></div>
            <?php if ($row['stock'] > 0): ?>
                <button class="add-to-cart-btn" onclick="openModal()">Add to Cart</button>
            <?php endif; ?>
        </div>
    </div>
</div>

<!-- Modal -->
<div id="cartModal" class="modal-overlay">
    <div class="modal">
        <h3>Add to Cart</h3>
        <form method="post">
            <label>Part: <?php echo htmlspecialchars($row['name']); ?></label><br>
            <label>Price: ₱<?php echo number_format($row['price'], 0); ?></label><br>
            <label>Stock: <?php echo $row['stock']; ?></label><br>
            <?php if ($row['branch'] === "BOTH BRANCHES"): ?>
                <label>Select Branch:</label>
                <select name="branch" id="branch">
                    <option value="ESTRADA STREET">ESTRADA STREET</option>
                    <option value="ARELLANO EXTENSION STREET">ARELLANO EXTENSION STREET</option>
                </select>
            <?php else: ?>
                <input type="hidden" name="branch" value="<?php echo $row['branch']; ?>">
            <?php endif; ?>

            <label>Quantity:</label>
            <input type="number" id="quantity" name="quantity" min="1" max="<?php echo $row['stock']; ?>" required oninput="updateTotal()" />
            <div id="errorMsg" class="error-msg"></div>
            <label>Total: ₱<span id="total">0</span></label><br><br>

            <button type="button" class="cancel-btn" onclick="closeModal()">Cancel</button>
            <button type="submit" name="addToCart" class="add-btn" onclick="validateCart()">Add to Cart</button>
        </form>
    </div>
</div>

<script>
function openModal() {
    document.getElementById('cartModal').style.display = 'flex';
}

function closeModal() {
    document.getElementById('cartModal').style.display = 'none';
}

function updateTotal() {
    const quantity = document.getElementById('quantity').value;
    const price = <?php echo $row['price']; ?>;
    const total = quantity * price;
    document.getElementById('total').textContent = total.toFixed(2);
}

function validateCart() {
    const quantity = document.getElementById('quantity').value;
    if (quantity <= 0 || quantity > <?php echo $row['stock']; ?>) {
        document.getElementById('errorMsg').textContent = "Invalid quantity.";
        return false;
    }
    return true;
}
</script>

</body>
</html>