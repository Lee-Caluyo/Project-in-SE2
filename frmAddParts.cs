using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmAddParts : Form
    {
        private string username;
        private frmParts parentForm;
        private int errorcount;
        Class1 newpart = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        // Path for cached and final images
        private string cachePath = @"C:\Users\caluy\OneDrive\Desktop\Project in SE2\PART CACHE";
        private string finalPath = @"C:\xampp\htdocs\ict127-CS2A-2024\Parts\PARTS IMAGE";
        private string cachedImagePath = ""; // Store the cached image path temporarily

        public frmAddParts(string username, frmParts parentForm)
        {
            InitializeComponent();
            this.username = username;
            this.parentForm = parentForm;
        }

        public void validateForm()
        {
            errorcount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtpartname.Text))
            {
                errorProvider1.SetError(txtpartname, "Input is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtprice.Text))
            {
                errorProvider1.SetError(txtprice, "Input is empty");
                errorcount++;
            }
            if (!int.TryParse(txtprice.Text, out _))
            {
                errorProvider1.SetError(txtprice, "Please enter a whole number");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtstock.Text))
            {
                errorProvider1.SetError(txtstock, "Input is empty");
                errorcount++;
            }
            if (cmbbranch.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbbranch, "Select Branch");
                errorcount++;
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                // ✅ Check if stock is below 20
                if (int.TryParse(txtstock.Text, out int stockValue) && stockValue < 20)
                {
                    MessageBox.Show("Warning: Stock is less than 20. You may want to review inventory levels.", "Low Stock Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                DialogResult dr = MessageBox.Show("Are you sure you want to add this part?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newpart.executeSQL("INSERT INTO tblproduct (name, branch, type, stock, price, DateCreated, CreatedBy) " +
                                           "VALUES ('" + txtpartname.Text + "', '" + cmbbranch.Text.ToUpper() + "', 'PARTS', '" +
                                           txtstock.Text + "', '" + txtprice.Text + "', '" + DateTime.Now.ToString("MM/dd/yyyy") +
                                           "', '" + username + "')");

                        if (newpart.rowAffected > 0)
                        {
                            newpart.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                               "VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                                               "', 'Add', 'Parts Management', '" + txtpartname.Text + "', '" + username + "')");

                            // If an image is uploaded, copy it to the final folder
                            if (!string.IsNullOrEmpty(cachedImagePath) && File.Exists(cachedImagePath))
                            {
                                Directory.CreateDirectory(finalPath); // Ensure the final path exists
                                string extension = Path.GetExtension(cachedImagePath);
                                string finalImagePath = Path.Combine(finalPath, txtpartname.Text + extension);

                                File.Copy(cachedImagePath, finalImagePath, true); // Copy and overwrite if exists
                                File.Delete(cachedImagePath); // Delete the cached image
                            }

                            MessageBox.Show("Part Added Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            parentForm.LoadData();
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            // Delete cached image if it exists
            if (!string.IsNullOrEmpty(cachedImagePath) && File.Exists(cachedImagePath))
            {
                try
                {
                    File.Delete(cachedImagePath); // Delete the cached image
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting cached image: " + ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Check if the cache folder is empty and delete it if so
            try
            {
                if (Directory.Exists(cachePath) && Directory.GetFiles(cachePath).Length == 0)
                {
                    Directory.Delete(cachePath); // Delete the empty cache folder
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting cache folder: " + ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.Close();
        }


        private void btnupload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpartname.Text))
            {
                MessageBox.Show("Please enter a part name before uploading an image.", "Missing Part Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtpartname.Focus();
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Directory.CreateDirectory(cachePath); // ensure cache folder exists

                    string extension = Path.GetExtension(ofd.FileName);
                    string newFileName = txtpartname.Text + extension;
                    cachedImagePath = Path.Combine(cachePath, newFileName);

                    File.Copy(ofd.FileName, cachedImagePath, true);

                    using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(cachedImagePath)))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error uploading image: " + ex.Message);
                }
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            txtpartname.Clear();
            txtprice.Clear();
            txtstock.Clear();
            cmbbranch.SelectedIndex = -1;
            errorProvider1.Clear();
            errorcount = 0;

            // Clear PictureBox image
            pictureBox1.Image = null;

            // Delete cached image if it exists
            if (!string.IsNullOrEmpty(cachedImagePath) && File.Exists(cachedImagePath))
            {
                try
                {
                    File.Delete(cachedImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting cached image: " + ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Clear the cached image path tracker
            cachedImagePath = "";
        }
    }
}
