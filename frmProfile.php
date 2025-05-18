<?php
session_start();

$profileDir = "/ict127-CS2A-2024/Profile/PROFILE";
$cacheDir = "/ict127-CS2A-2024/Profile/PROFILE CACHE";

if (!isset($_SESSION['username'])) {
    header("Location: frmLogin.php");
    exit;
}

$conn = new mysqli("localhost", "lee", "12345", "cs311a2024");
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

function getProfileImage($username) {
    global $profileDir, $cacheDir;
    $extensions = ['jpg', 'jpeg', 'png'];

    // Check cache directory first
    foreach ($extensions as $ext) {
        $cachePath = $_SERVER['DOCUMENT_ROOT'] . $cacheDir . '/' . $username . '.' . $ext;
        if (file_exists($cachePath)) {
            return $cacheDir . '/' . $username . '.' . $ext;
        }
    }
    
    // Then check profile directory
    foreach ($extensions as $ext) {
        $profilePath = $_SERVER['DOCUMENT_ROOT'] . $profileDir . '/' . $username . '.' . $ext;
        if (file_exists($profilePath)) {
            return $profileDir . '/' . $username . '.' . $ext;
        }
    }

    // Default image if none exists
    return "data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='100' height='100'><text x='50%' y='50%' text-anchor='middle' font-size='40' dy='.3em'>👤</text></svg>";
}

$username = $_SESSION['username'];
$profileImage = getProfileImage($username);

// Get birthdate and email for UI display
$birthdate = "";
$email = "";
$result = $conn->query("SELECT birthdate, email FROM tbluser WHERE username = '" . $conn->real_escape_string($username) . "'");
if ($result && $row = $result->fetch_assoc()) {
    $birthdate = trim($row['birthdate']);
    $email = trim($row['email']);
}

// --- Handle Profile Image Upload ---
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['upload_image'])) {
    if (isset($_FILES['profile_image']) && $_FILES['profile_image']['error'] === UPLOAD_ERR_OK) {
        // Use the absolute path as specified
        $targetDir = "C:/xampp/htdocs/ict127-CS2A-2024/Profile/PROFILE/";
        $allowedExtensions = ['jpg', 'jpeg', 'png'];
        $imageFile = $_FILES['profile_image'];

        // Validate that the file is an image
        $check = getimagesize($imageFile['tmp_name']);
        if ($check === false) {
            echo "<script>alert('The uploaded file is not a valid image.');</script>";
        } else {
            $uploadedExt = strtolower(pathinfo($imageFile['name'], PATHINFO_EXTENSION));
            if (!in_array($uploadedExt, $allowedExtensions)) {
                echo "<script>alert('Unsupported image extension. Allowed extensions are: jpg, jpeg, png.');</script>";
            } else {
                // Delete any existing image for this user
                foreach ($allowedExtensions as $ext) {
                    $existingFile = $targetDir . $username . '.' . $ext;
                    if (file_exists($existingFile)) {
                        unlink($existingFile);
                    }
                }
                
                // Define target file name which is the username with the new extension
                $targetFile = $targetDir . $username . '.' . $uploadedExt;
                
                if (move_uploaded_file($imageFile['tmp_name'], $targetFile)) {
                    echo "<script>alert('Profile image updated successfully.');</script>";
                    echo "<script>location.reload();</script>";
                } else {
                    echo "<script>alert('Failed to upload the new image.');</script>";
                }
            }
        }
    } else {
        echo "<script>alert('Please select a valid image file.');</script>";
    }
}

// --- Handle Password Change ---
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['change_password'])) {
    $enteredCurrent = $_POST['current_password'] ?? '';
    $newPassword = $_POST['new_password'] ?? '';
    $confirmPassword = $_POST['confirm_password'] ?? '';

    $sql = "SELECT password FROM tbluser WHERE username = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("s", $username);
    $stmt->execute();
    $stmt->bind_result($storedPassword);
    $stmt->fetch();
    $stmt->close();

    if ($enteredCurrent === $newPassword) {
        echo "<script>alert('New password cannot be the same as the current password.');</script>";
    } else {
        if ($enteredCurrent === $storedPassword) {
            if ($newPassword === $confirmPassword) {
                $update = $conn->prepare("UPDATE tbluser SET password = ? WHERE username = ?");
                $update->bind_param("ss", $newPassword, $username);
                if ($update->execute()) {
                    echo "<script>alert('Password updated successfully.');</script>";
                    echo "<script>setTimeout(function(){ location.reload(); }, 1000);</script>";
                } else {
                    echo "<script>alert('Failed to update password.');</script>";
                }
                $update->close();
            } else {
                echo "<script>alert('New passwords do not match.');</script>";
            }
        }
    }
}

// --- Update Birthdate ---
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['update_birthdate'])) {
    $birthdate = $_POST['birthdate'] ?? '';
    if (!empty($birthdate)) {
        $update = $conn->prepare("UPDATE tbluser SET birthdate = ? WHERE username = ?");
        $update->bind_param("ss", $birthdate, $username);
        if ($update->execute()) {
            echo "<script>alert('Birthdate updated successfully.');</script>";
        } else {
            echo "<script>alert('Failed to update birthdate.');</script>";
        }
        $update->close();
    } else {
        echo "<script>alert('Please enter a valid birthdate.');</script>";
    }
}

// --- Update Email ---
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['update_email'])) {
    $newEmail = $_POST['new_email'] ?? '';

    if (!empty($newEmail)) {
        $query = $conn->prepare("SELECT email FROM tbluser WHERE username = ?");
        $query->bind_param("s", $username);
        $query->execute();
        $query->bind_result($currentEmail);
        $query->fetch();
        $query->close();

        if ($newEmail !== $currentEmail) {
            $update = $conn->prepare("UPDATE tbluser SET email = ? WHERE username = ?");
            $update->bind_param("ss", $newEmail, $username);
            if ($update->execute()) {
                echo "<script>
                    alert('Email updated successfully.');
                    location.reload();
                </script>";
            } else {
                echo "<script>alert('Failed to update email.');</script>";
            }
            $update->close();
        } else {
            echo "<script>alert('No changes made to email.');</script>";
        }
    } else {
        echo "<script>alert('Please enter a valid email address.');</script>";
    }
}
?>




<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Profile</title>
    <style>
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgba(0,0,0,0.4);
  z-index: 1000;
}
.modal-content {
  background: white;
  padding: 20px 30px;
  border-radius: 10px;
  width: 500px;
  text-align: center;
}
.hidden {
  display: none;
}

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
            width: 40px;
            height: 40px;
            object-fit: cover;
            border-radius: 50%;
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
              window.location.href = "logout.php";
          } else {
              event.preventDefault();
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
                <img src="Icons/cart_icon.png" alt="Cart">
            </a>
        </div>

        <a href="frmProfile.php">
            <img src="<?php echo $profileImage; ?>" alt="Profile Icon">
        </a>
    </div>
</div>


<div class="content">
<div style="background-color: #82c9f4; padding: 40px; border-radius: 30px; width: 60%; max-width: 700px;">
    <div style="display: flex; flex-direction: column; align-items: center;">
        <div style="background-color: black; border-radius: 50%; width: 150px; height: 150px; display: flex; align-items: center; justify-content: center; overflow: hidden;">
            <img id="triggerFile" src="<?php echo $profileImage; ?>" alt="Profile Icon" style="width: 100%; height: 100%; object-fit: cover; cursor: pointer;">
            <form method="POST" enctype="multipart/form-data" id="autoUploadForm">
                <input type="file" id="fileInput" name="profile_pic" accept="image/*" style="display: none;" onchange="document.getElementById('autoUploadForm').submit();">
                <input type="hidden" name="upload_temp" value="1">
            </form>
        </div>
        <button onclick="document.getElementById('fileInput').click();" 
                style="margin-top: 15px; background-color: #5f3dc4; color: white; padding: 10px 20px; border: none; border-radius: 10px; cursor: pointer;">
            Upload Profile Picture
        </button>
    </div>

    <div style="margin-top: 30px; width: 100%;">
        <div style="background-color: white; padding: 15px 20px; border-radius: 20px; margin-bottom: 15px;">
            <?= $username; ?>
        </div>
        <div onclick="openModal('modalPassword')" style="background-color: #5f3dc4; color: white; padding: 15px 20px; border-radius: 20px; margin-bottom: 15px; cursor: pointer;">
            Change Password
        </div>
        <div onclick="openModal('modalBirthdate')" style="background-color: white; padding: 15px 20px; border-radius: 20px; margin-bottom: 15px; cursor: pointer;">
            <?= $birthdate !== '' ? $birthdate : 'Add Birthdate'; ?>
        </div>
        <div onclick="openModal('modalEmail')" style="background-color: white; padding: 15px 20px; border-radius: 20px; cursor: pointer;">
            <?= $email !== '' ? $email : 'Add Email'; ?>
        </div>
    </div>
</div>
</div>

<!-- Append modal HTML to the bottom of current HTML -->

<!-- CHANGE PASSWORD MODAL -->
<div id="modalPassword" class="modal hidden">
  <div class="modal-content">
    <h3>Change Password</h3>
    <form method="POST">
      <input type="password" id="current_password" name="current_password" placeholder="Current Password" required 
        style="margin-bottom: 10px; width: 80%; padding: 8px;">
      <input type="checkbox" onclick="togglePassword()"> <br>
      <input type="password" name="new_password" placeholder="New Password" required style="margin-bottom: 10px; width: 85%; padding: 8px;"><br>
      <input type="password" name="confirm_password" placeholder="Confirm New Password" required style="margin-bottom: 10px; width: 85%; padding: 8px;"><br>
      <button type="submit" name="change_password" style="background-color: #5f3dc4; color: white; padding: 8px 15px; border: none; border-radius: 8px;">Submit</button>
      <button type="button" onclick="closeModal('modalPassword')" style="background-color: #5f3dc4; color: white; padding: 8px 15px; border: none; border-radius: 8px; margin-left: 10px;">Cancel</button>
    </form>
  </div>
</div>

<!-- ADD BIRTHDATE MODAL -->
<div id="modalBirthdate" class="modal hidden">
  <div class="modal-content">
    <h3>Add Birthdate</h3>
    <form method="POST">
      <input type="date" name="birthdate" required style="margin-bottom: 10px; width: 95%; padding: 8px;" value="<?= htmlspecialchars($birthdate) ?>"><br>
      <button type="submit" name="update_birthdate" style="background-color: #5f3dc4; color: white; padding: 8px 15px; border: none; border-radius: 8px;">Submit</button>
      <button type="button" onclick="closeModal('modalBirthdate')" style="background-color: #5f3dc4; color: white; padding: 8px 15px; border: none; border-radius: 8px; margin-left: 10px;">Cancel</button>
    </form>
  </div>
</div>

<!-- ADD EMAIL MODAL -->
<div id="modalEmail" class="modal hidden">
  <div class="modal-content">
    <h3>Add Email</h3>
    <form method="POST">
      <input type="email" name="new_email" placeholder="New Email" required style="margin-bottom: 10px; width: 95%; padding: 8px;" value="<?= htmlspecialchars($email) ?>"><br>
      <button type="submit" name="update_email" style="background-color: #5f3dc4; color: white; padding: 8px 15px; border: none; border-radius: 8px;">Submit</button>
      <button type="button" onclick="closeModal('modalEmail')" style="background-color: #5f3dc4; color: white; padding: 8px 15px; border: none; border-radius: 8px; margin-left: 10px;">Cancel</button>
    </form>
  </div>
</div>




<script>
function openModal(id) {
  document.getElementById(id).classList.remove('hidden');
}
function closeModal(id) {
  document.getElementById(id).classList.add('hidden');
}
function togglePassword() {
  const x = document.getElementById("current_password");
  if (x.type === "password") x.type = "text"; else x.type = "password";
}
</script>


</body>
</html>
