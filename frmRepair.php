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

$sql = "SELECT concern, branch, method1, method2, code, mechanic1, mechanic2, bikerecieving, bikestatus, pickupdropoff, address, contact, servicefee, repairfee, price, DateRequested, DateComplete, DateDelivered, DateRecieved 
        FROM tblrepair 
        WHERE username = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $username);
$stmt->execute();
$result = $stmt->get_result();

$repairItems = [];
while ($row = $result->fetch_assoc()) {
    $repairItems[] = $row;
}
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>My Repairs</title>
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
        th, td { padding: 12px; border: 1px solid #ccc; text-align: left; font-size: 15px; cursor: pointer; }
        .no-items { text-align: center; margin-top: 20px; color: #555; }
        .logged-in { text-align: center; font-weight: bold; color: #444; }
        .cart img { width: 40px; height: 40px; cursor: pointer; }

        .add-request-btn {
            padding: 12px 24px;
            font-size: 16px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            margin: 20px auto;
            display: block;
        }

        .modal {
            display: none; position: fixed; z-index: 10;
            left: 0; top: 0; width: 100%; height: 100%;
            overflow: auto; background-color: rgba(0,0,0,0.5);
        }

        .modal-content {
            background-color: white;
            margin: 10% auto;
            padding: 20px;
            border-radius: 12px;
            width: 60%;
        }

        .modal-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modal-header h3 {
            margin: 0;
        }

        .close { font-size: 24px; font-weight: bold; cursor: pointer; }

        .modal-table td {
            padding: 6px;
        }
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
            <a href="frmCart.php" title="Go to Cart">
                <img src="Icons/cart_icon.png" alt="Cart">
            </a>
        </div>

        <a href="frmProfile.php">
            <img src="<?php echo $profileImage; ?>" alt="Profile Icon">
        </a>
    </div>
</div>

<h2>Repair Requests</h2>
<div class="logged-in">Logged in as: <?= htmlspecialchars($username) ?></div>

<a href="frmAddRepair.php">
    <button class="add-request-btn">Add Request</button>
</a>

<div class="search-container">
    <input type="text" id="searchInput" onkeyup="filterTable()" placeholder="Search concern...">
</div>

<?php if (count($repairItems) > 0): ?>
    <table id="repairTable">
        <thead>
            <tr>
                <th>Concern</th>
                <th>Branch</th>
                <th>Request Method</th>
                <th>Delivery Method</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
        <?php foreach ($repairItems as $item): ?>
            <tr class="repair-row" 
                data-concern="<?= htmlspecialchars($item['concern']) ?>"
                data-branch="<?= htmlspecialchars($item['branch']) ?>"
                data-method1="<?= htmlspecialchars($item['method1']) ?>"
                data-method2="<?= htmlspecialchars($item['method2']) ?>"
                data-code="<?= htmlspecialchars($item['code']) ?>"
                data-mechanic1="<?= htmlspecialchars($item['mechanic1']) ?>"
                data-mechanic2="<?= htmlspecialchars($item['mechanic2']) ?>"
                data-bikerecieving="<?= htmlspecialchars($item['bikerecieving']) ?>"
                data-bikestatus="<?= htmlspecialchars($item['bikestatus']) ?>"
                data-pickupdropoff="<?= htmlspecialchars($item['pickupdropoff']) ?>"
                data-address="<?= htmlspecialchars($item['address']) ?>"
                data-contact="<?= htmlspecialchars($item['contact']) ?>"
                data-servicefee="<?= htmlspecialchars($item['servicefee']) ?>"
                data-repairfee="<?= htmlspecialchars($item['repairfee']) ?>"
                data-price="<?= htmlspecialchars($item['price']) ?>"
                data-daterequested="<?= htmlspecialchars($item['DateRequested']) ?>"
                data-datecomplete="<?= htmlspecialchars($item['DateComplete']) ?>"
                data-datedelivered="<?= htmlspecialchars($item['DateDelivered']) ?>"
                data-daterecieved="<?= htmlspecialchars($item['DateRecieved']) ?>">
                <td><?= htmlspecialchars($item['concern']) ?></td>
                <td><?= htmlspecialchars($item['branch']) ?></td>
                <td><?= htmlspecialchars($item['method1']) ?></td>
                <td><?= htmlspecialchars($item['method2']) ?></td>
                <td><?= htmlspecialchars($item['bikestatus']) ?></td>
            </tr>
        <?php endforeach; ?>
        </tbody>
    </table>
<?php else: ?>
    <div class="no-items">You have no repair requests.</div>
<?php endif; ?>

<div id="detailModal" class="modal">
    <div class="modal-content">
        <div class="modal-header">
            <h3>Repair Details</h3>
            <span class="close" onclick="closeModal()">&times;</span>
        </div>
        <table class="modal-table" id="modalTable">
        </table>
    </div>
</div>

<script>
function filterTable() {
    const input = document.getElementById("searchInput").value.toLowerCase();
    const rows = document.querySelectorAll("#repairTable tbody tr");
    rows.forEach(row => {
        const concern = row.cells[0].textContent.toLowerCase();
        row.style.display = concern.includes(input) ? "" : "none";
    });
}

function closeModal() {
    document.getElementById('detailModal').style.display = 'none';
}

document.querySelectorAll('.repair-row').forEach(row => {
    row.addEventListener('click', () => {
        const fields = [
            "concern", "branch", "method1", "method2", "code",
            "mechanic1", "mechanic2", "bikerecieving", "bikestatus",
            "pickupdropoff", "address", "contact",
            "servicefee", "repairfee", "price",
            "daterequested", "datecomplete", "datedelivered", "daterecieved"
        ];

        const modalTable = document.getElementById('modalTable');
        modalTable.innerHTML = "";
        fields.forEach(field => {
            const label = field.replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase());
            const value = row.getAttribute('data-' + field);
            modalTable.innerHTML += `<tr><td><strong>${label}</strong></td><td>${value}</td></tr>`;
        });

        document.getElementById('detailModal').style.display = 'block';
    });
});
</script>

</body>
</html>