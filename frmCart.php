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

// Handle purchase confirmation
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['confirm_purchase'])) {
    $method = $_POST['method'];
    $address = $_POST['address'];
    $buyer = $_SESSION['username'];

    if ($method) {
        $update = $conn->prepare("UPDATE tblcart SET status='IN PURCHASE', method2=?, DatePurchased=NOW() WHERE buyer=? AND status='IN CART'");
        $update->bind_param("ss", $method, $buyer);

        if ($update->execute()) {
            $purchaseSuccess = true;
        } else {
            $purchaseError = true;
        }
        $update->close();
    }
}

function getProfileImage($username) {
    $profileDir = "/ict127-CS2A-2024/Profile/PROFILE";
    $extensions = ['jpg', 'jpeg', 'png'];

    foreach ($extensions as $ext) {
        $profileImagePath = $_SERVER['DOCUMENT_ROOT'] . $profileDir . '/' . $username . '.' . $ext;
        if (file_exists($profileImagePath)) {
            return $profileDir . '/' . $username . '.' . $ext;
        }
    }

    return "data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='100' height='100'><text x='50%' y='50%' text-anchor='middle' font-size='40' dy='.3em'>👤</text></svg>";
}

$username = $_SESSION['username'];
$profileImage = getProfileImage($username);

$sql = "SELECT buyer, name, branch, price, type, contact, method2, status, quantity, code, specs, DateAdded, DatePurchased 
        FROM tblcart 
        WHERE (status = 'IN CART' OR status = 'FOR PURCHASE') AND buyer = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $username);
$stmt->execute();
$result = $stmt->get_result();

$cartItems = [];
while ($row = $result->fetch_assoc()) {
    $cartItems[] = $row;
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>My Cart</title>
    <style>
        body { font-family: 'Arial Rounded MT Bold', sans-serif; background-color: #d3f0ff; margin: 0; }
        .navbar { background-color: #5f3dc4; padding: 20px; border-radius: 0 0 30px 30px; display: flex; align-items: center; justify-content: space-between; color: white; }
        .navbar a { color: white; font-weight: bold; text-decoration: none; margin-right: 20px; }
        .navbar .user-area { display: flex; align-items: center; gap: 15px; }
        .navbar .user-area img { width: 40px; height: 40px; object-fit: cover; border-radius: 50%; }
        .search-container { text-align: center; margin: 20px 0; }
        .search-container input[type="text"] { padding: 10px; width: 320px; font-size: 16px; border: 1px solid #ccc; border-radius: 4px; }
        h2 { text-align: center; font-size: 28px; }
        table { width: 95%; margin: 0 auto; border-collapse: collapse; background-color: rgba(255, 255, 255, 0.95); }
        th, td { padding: 12px; border: 1px solid #ccc; text-align: left; font-size: 15px; }
        th.hidden, td.hidden { display: none; }
        .no-items { text-align: center; margin-top: 20px; color: #555; }
        .logged-in { text-align: center; font-weight: bold; color: #444; }
        .cart img { width: 40px; height: 40px; cursor: pointer; }
        .modal { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0,0,0,0.6); display: none; justify-content: center; align-items: center; z-index: 1000; }
        .modal-content { background: white; padding: 20px; border-radius: 15px; width: 400px; text-align: center; }
        .modal-content button { margin: 10px; padding: 10px 15px; font-weight: bold; border: none; border-radius: 8px; cursor: pointer; }
        button.purchase-btn { background-color: #28a745; color: white; }
        button.purchase-btn:hover { background-color: #218838; }
        button.remove-btn { background-color: #ff6666; color: white; }
        button.remove-btn:hover { background-color: #e04f4f; }
        pre { text-align: center; white-space: pre-wrap; word-wrap: break-word; }
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
        <div class="cart">
            <a href="frmCart.php" title="Go to Cart"><img src="Icons/cart_icon.png" alt="Cart"></a>
        </div>
        <a href="frmProfile.php"><img src="<?php echo $profileImage; ?>" alt="Profile Icon"></a>
    </div>
</div>

<h2>Cart</h2>
<div class="logged-in">Logged in as: <?= htmlspecialchars($username) ?></div>

<div class="search-container">
    <input type="text" id="searchInput" onkeyup="filterTable()" placeholder="Search item...">
</div>

<?php if (count($cartItems) > 0): ?>
    <table id="cartTable">
        <thead>
        <tr>
            <th>Name</th><th>Branch</th><th>Price</th><th>Type</th>
            <th class="hidden">Contact</th><th class="hidden">Method2</th>
            <th>Status</th><th>Quantity</th>
            <th class="hidden">Code</th><th class="hidden">Specs</th>
            <th>Date Added</th><th class="hidden">Date Purchased</th>
        </tr>
        </thead>
        <tbody>
        <?php foreach ($cartItems as $item): ?>
            <tr>
                <td><?= htmlspecialchars($item['name']) ?></td>
                <td><?= htmlspecialchars($item['branch']) ?></td>
                <td><?= htmlspecialchars($item['price']) ?></td>
                <td><?= htmlspecialchars($item['type']) ?></td>
                <td class="hidden"><?= htmlspecialchars($item['contact']) ?></td>
                <td class="hidden"><?= htmlspecialchars($item['method2']) ?></td>
                <td><?= htmlspecialchars($item['status']) ?></td>
                <td><?= htmlspecialchars($item['quantity']) ?></td>
                <td class="hidden"><?= htmlspecialchars($item['code']) ?></td>
                <td class="hidden"><?= htmlspecialchars($item['specs']) ?></td>
                <td><?= htmlspecialchars($item['DateAdded']) ?></td>
                <td class="hidden"><?= htmlspecialchars($item['DatePurchased']) ?></td>
            </tr>
        <?php endforeach; ?>
        </tbody>
    </table>
<?php else: ?>
    <div class="no-items">Your cart is empty.</div>
<?php endif; ?>

<!-- Modals -->
<div id="modalPart" class="modal">
    <div class="modal-content">
        <h3>Part Details</h3>
        <p><strong>Part Name:</strong> <span id="partName"></span></p>
        <p><strong>Price:</strong> ₱<span id="partPrice"></span></p>
        <p><strong>Quantity:</strong> <span id="partQuantity"></span></p>
        <button onclick="closeModal()">Close</button>
        <button class="remove-btn" onclick="removeFromCart()">Remove from Cart</button>
        <button class="purchase-btn" onclick="purchase()">Purchase</button>
    </div>
</div>

<div id="modalBuilt" class="modal">
    <div class="modal-content">
        <h3>Bike Details</h3>
        <p><strong>Bike Name:</strong> <span id="bikeName"></span></p>
        <p><strong>Price:</strong> ₱<span id="bikePrice"></span></p>
        <p><strong>Status:</strong> <span id="bikeStatus"></span></p>
        <p><strong>Branch:</strong> <span id="bikeBranch"></span></p>
        <p><strong>Specifications:</strong></p>
        <pre id="bikeSpecs"></pre>
        <button onclick="closeModal()">Close</button>
        <button class="remove-btn" onclick="removeFromCart()">Remove from Cart</button>
        <button class="purchase-btn" onclick="purchase()">Purchase</button>
    </div>
</div>

<div id="methodModal" class="modal">
    <div class="modal-content">
        <h3>Purchase</h3>
        <label>Method:</label>
        <select id="methodSelect" onchange="toggleAddressInput()">
            <option value="" disabled selected>----Select Method----</option>
            <option value="PICK UP">PICK UP</option>
            <option value="DELIVERY">DELIVERY</option>
        </select>
        <br><br>
        <label>Enter Address:</label>
        <textarea id="addressInput" rows="4" disabled></textarea>
        <br><br>
        <div style="display: flex; justify-content: space-between;">
            <button onclick="closeMethodModal()" style="background-color: #ff6666;">Cancel</button>
            <button onclick="confirmPurchase()" style="background-color: #28a745;">Confirm Purchase</button>
        </div>
    </div>
</div>

<?php if (isset($purchaseSuccess)): ?>
    <script>alert("Purchase confirmed!"); location.reload();</script>
<?php elseif (isset($purchaseError)): ?>
    <script>alert("Purchase failed. Try again.");</script>
<?php endif; ?>

<form id="purchaseForm" method="POST" style="display: none;">
    <input type="hidden" name="confirm_purchase" value="1">
    <input type="hidden" name="method" id="hiddenMethod">
    <input type="hidden" name="address" id="hiddenAddress">
</form>

<script>
function filterTable() {
    const input = document.getElementById("searchInput").value.toLowerCase();
    const rows = document.querySelectorAll("#cartTable tbody tr");
    rows.forEach(row => {
        const name = row.cells[0].textContent.toLowerCase();
        row.style.display = name.includes(input) ? "" : "none";
    });
}
function closeModal() {
    document.getElementById("modalPart").style.display = "none";
    document.getElementById("modalBuilt").style.display = "none";
}
function closeMethodModal() {
    document.getElementById("methodModal").style.display = "none";
}
 function removeFromCart() {
        alert("Removing from cart...");
        // TODO: Add PHP functionality to remove item from the cart
    }

function purchase() {
    document.getElementById("methodModal").style.display = "flex";
}
function toggleAddressInput() {
    const method = document.getElementById("methodSelect").value;
    const addressInput = document.getElementById("addressInput");
    addressInput.disabled = (method === "PICK UP");
    if (method === "PICK UP") addressInput.value = "";
}
function confirmPurchase() {
    const method2 = document.getElementById("methodSelect").value;
    const address = document.getElementById("addressInput").value;
    if (!method2) {
        alert("Please select a purchase method.");
        return;
    }
    if (method2 === "DELIVERY" && address.trim() === "") {
        alert("Please enter your delivery address.");
        return;
    }
    document.getElementById("hiddenMethod").value = method2;
    document.getElementById("hiddenAddress").value = address;
    document.getElementById("purchaseForm").submit();
}
document.querySelectorAll("#cartTable tbody tr").forEach(row => {
    row.addEventListener("click", () => {
        const type = row.cells[3].textContent.trim();
        if (type === "PARTS") {
            document.getElementById("partName").textContent = row.cells[0].textContent;
            document.getElementById("partPrice").textContent = row.cells[2].textContent;
            document.getElementById("partQuantity").textContent = row.cells[7].textContent;
            document.getElementById("modalPart").style.display = "flex";
        } else if (type === "BUILT") {
            document.getElementById("bikeName").textContent = row.cells[0].textContent;
            document.getElementById("bikePrice").textContent = row.cells[2].textContent;
            document.getElementById("bikeStatus").textContent = row.cells[6].textContent;
            document.getElementById("bikeBranch").textContent = row.cells[1].textContent;
            document.getElementById("bikeSpecs").textContent = row.cells[9].textContent;
            document.getElementById("modalBuilt").style.display = "flex";
        }
    });
});
</script>

</body>
</html>
