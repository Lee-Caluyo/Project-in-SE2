using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmViewParts : Form
    {
        private frmParts parentForm;
        private int price, quantity, errorcount;

        private string username, contact, usertype, viewpartname, viewpartprice, viewpartbranch, viewpartstock;

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        Class1 addcartpart = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        public frmViewParts(frmParts parent, string username, string contact, string usertype, string viewpartname, string viewpartbranch, string viewpartstock, string viewpartprice)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;
            this.viewpartname = viewpartname;
            this.viewpartprice = viewpartprice;
            this.viewpartbranch = viewpartbranch;
            this.viewpartstock = viewpartstock;
        }
        public void validateForm()
        { 
            errorProvider1.Clear();
            if (!int.TryParse(txtquantity.Text, out quantity))
            {
                errorProvider1.SetError(txtquantity, "Quantity must be a number.");
                return;
            }
            if (quantity <= 0)
            {
                errorProvider1.SetError(txtquantity, "Quantity must be greater than zero.");
                return;
            }
            if (!int.TryParse(txtstock.Text, out int stock))
            {
                errorProvider1.SetError(txtstock, "Invalid stock value.");
                return;
            }
            if (quantity > stock)
            {
                errorProvider1.SetError(txtquantity, "Quantity cannot exceed available stock.");
                return;
            }
        }
        private void frmViewParts_Load(object sender, EventArgs e)
        {
            txtpartname.Text = viewpartname;
            txtbranch.Text = viewpartbranch;
            txtprice.Text = viewpartprice;
            txtstock.Text = viewpartstock;


            if (usertype == "STAFF" || usertype == "MANAGER" || usertype == "OWNER")
            {
                btnaddtocart.Hide();
                txtquantity.Enabled = false;
            }

            try
            {
                string imageFolder = @"C:\xampp\htdocs\ict127-CS2A-2024\Parts\PARTS IMAGE\";
                string[] extensions = { ".jpg", ".jpeg", ".png" };
                string partname = viewpartname;

                bool imageFound = false;

                foreach (string ext in extensions)
                {
                    string imagePath = Path.Combine(imageFolder, partname + ext);
                    if (File.Exists(imagePath))
                    {
                        using (var imgTemp = Image.FromFile(imagePath))
                        {
                            pictureBox1.Image = new Bitmap(imgTemp);
                        }
                        imageFound = true;
                        break;
                    }
                }

                if (!imageFound)
                {
                    pictureBox1.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                errorProvider1.SetError(txtquantity, "Only numbers are allowed!");
            }
            else
            {
                errorProvider1.SetError(txtquantity, "");
            }
        }
        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtquantity.Text, out int quantity) && int.TryParse(txtprice.Text, out int price))
            {
                // Update total price
                txttotalprice.Text = (quantity * price).ToString();
            }
            else
            {
                txttotalprice.Text = "0"; // Reset if invalid input
            }
        }
        private void btnaddtocart_Click(object sender, EventArgs e)
        {
             validateForm();

            DialogResult dr = MessageBox.Show("Are you sure you want to add this to your cart?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataTable dt = addcartpart.GetData($"SELECT contact FROM tbluser WHERE username = '{username}'");
                    string contact = dt.Rows.Count > 0 ? dt.Rows[0]["contact"].ToString() : "No Contact Found";

                    string codePrefix = txtbranch.Text == "Arellano Extension Street" ? "AES" :
                                        txtbranch.Text == "Estrada Street" ? "ES" : "UNK";

                    string insertCartQuery = $"INSERT INTO tblcart (buyer, name, price, branch, type, contact, status, code, quantity, DateAdded, DatePurchased) " +
                                             $"VALUES ('{username}', '{txtpartname.Text}', '{txtprice.Text}', '{txtbranch.Text}', 'PARTS', '{contact}', 'IN CART', '', '{txtquantity.Text}', '{DateTime.Now:yyyy-MM-dd}', '')";

                    addcartpart.executeSQL(insertCartQuery);

                    if (addcartpart.rowAffected > 0)
                    {
                        // Reduce stock in database
                        string updateStockQuery = $"UPDATE tblproduct SET stock = stock - {txtquantity.Text} " +
                                                  $"WHERE name = '{txtpartname.Text}' AND branch = '{txtbranch.Text}' AND type = 'PARTS'";
                        addcartpart.executeSQL(updateStockQuery);

                        // Update UI stock value
                        if (int.TryParse(txtstock.Text, out int currentStock) && int.TryParse(txtquantity.Text, out int quantity))
                        {
                            txtstock.Text = (currentStock - quantity).ToString();
                        }

                        string insertLogQuery = $"INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                                $"VALUES ('{DateTime.Now.ToShortDateString()}', '{DateTime.Now.ToShortTimeString()}', " +
                                                $"'Add', 'Adding to Cart (Part)', '{txtpartname.Text}', '{username}')";

                        addcartpart.executeSQL(insertLogQuery);

                        MessageBox.Show("Added to Cart!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        parentForm?.LoadData();
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
}
