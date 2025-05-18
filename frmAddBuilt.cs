using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmAddBuilt : Form
    {
        string username;
        int errorcount;
        private frmBuilt parentForm;
        Class1 newbike = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        private string cachePath = @"C:\Users\caluy\OneDrive\Desktop\Project in SE2\BUILT CACHE";
        private string finalPath = @"C:\xampp\htdocs\ict127-CS2A-2024\Built\BUILT IMAGE";
        private string cachedImagePath = "";

        public frmAddBuilt(string username, frmBuilt parentForm)
        {
            InitializeComponent();
            this.username = username;
            this.parentForm = parentForm;
        }

        public void validateForm()
        {
            errorcount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtbikename.Text))
            {
                errorProvider1.SetError(txtbikename, "Input is empty");
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
            if (string.IsNullOrEmpty(txtbikespecs.Text))
            {
                errorProvider1.SetError(txtbikespecs, "Input is empty");
                errorcount++;
            }
            if (cmbbranch.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbbranch, "Select Branch");
                errorcount++;
            }

            try
            {
                DataTable dt = newbike.GetData("SELECT * FROM tblproduct WHERE name = '" + txtbikename.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtbikename, "Bike name is already in use.");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing username", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this bike?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newbike.executeSQL("INSERT INTO tblproduct (name, branch, type, stock, specs, price, SaleStatus, DateCreated, CreatedBy) " +
                            "VALUES ('" + txtbikename.Text + "', '" + cmbbranch.Text.ToUpper() + "', 'BUILT', '1', '" +
                            txtbikespecs.Text + "', '" + txtprice.Text + "', 'FOR SALE', '" +
                            DateTime.Now.ToString("MM/dd/yyyy") + "', '" + username + "')");

                        if (newbike.rowAffected > 0)
                        {
                            newbike.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                "VALUES ('" + DateTime.Now.ToShortDateString() + "', '" +
                                DateTime.Now.ToShortTimeString() + "', 'Add', 'Bike Management', '" +
                                txtbikename.Text + "', '" + username + "')");

                            if (!string.IsNullOrEmpty(cachedImagePath) && File.Exists(cachedImagePath))
                            {
                                Directory.CreateDirectory(finalPath);
                                string extension = Path.GetExtension(cachedImagePath);
                                string finalImagePath = Path.Combine(finalPath, txtbikename.Text + extension);

                                File.Copy(cachedImagePath, finalImagePath, true);
                                File.Delete(cachedImagePath);
                            }

                            MessageBox.Show("Bike Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            txtbikename.Clear();
            txtprice.Clear();
            txtbikespecs.Clear();
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
            if (string.IsNullOrWhiteSpace(txtbikename.Text))
            {
                MessageBox.Show("Please enter a bike name before uploading an image.", "Missing Bike Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbikename.Focus();
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Directory.CreateDirectory(cachePath);

                    string extension = Path.GetExtension(ofd.FileName);
                    string newFileName = txtbikename.Text + extension;
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
    }
}
