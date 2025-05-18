using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmViewBuilt : Form
    {
        private frmBuilt parentForm;
        private string username, usertype, contact, viewbikename, viewbikeprice, viewbikebranch, viewbikespecs, viewsalestatus;
        Class1 addcartbuilt = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        public frmViewBuilt(frmBuilt parent, string username, string usertype, string contact, string viewbikename, string viewbikebranch, string viewbikespecs, string viewbikeprice, string viewsalestatus)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.username = username;
            this.usertype = usertype;
            this.contact = contact;
            this.viewbikename = viewbikename;
            this.viewbikeprice = viewbikeprice;
            this.viewbikebranch = viewbikebranch;
            this.viewbikespecs = viewbikespecs;
            this.viewsalestatus = viewsalestatus;
        }
        private void btnaddtocart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to add this to your cart?",
                                              "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataTable dt = addcartbuilt.GetData($"SELECT contact FROM tbluser WHERE username = '{username}'");
                    string userContact = dt.Rows.Count > 0 ? dt.Rows[0]["contact"].ToString() : "No Contact Found";

                    string codePrefix = txtbranch.Text == "Arellano Extension Street" ? "AES" :
                                        txtbranch.Text == "Estrada Street" ? "ES" : "UNK";

                    string insertCartQuery = $"INSERT INTO tblcart (buyer, name, branch, price, type, contact, status, quantity, code, DateAdded, DatePurchased) " +
                         $"VALUES ('{username}', '{txtbikename.Text}', '{txtbranch.Text}', '{txtprice.Text}', 'BUILT', '{userContact}', 'IN CART', 1, '', '{DateTime.Now:yyyy-MM-dd}', '')";


                    addcartbuilt.executeSQL(insertCartQuery);

                    if (addcartbuilt.rowAffected > 0)
                    {
                        string updateProductQuery = $"UPDATE tblproduct SET SaleStatus = 'BEING PURCHASED' " +
                                                    $"WHERE name = '{txtbikename.Text}' AND branch = '{txtbranch.Text}' AND type = 'BUILT'";

                        addcartbuilt.executeSQL(updateProductQuery);

                        string insertLogQuery = $"INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                                $"VALUES ('{DateTime.Now.ToShortDateString()}', '{DateTime.Now.ToShortTimeString()}', " +
                                                $"'Add', 'Adding to Cart (Built)', '{txtbikename.Text}', '{username}')";

                        addcartbuilt.executeSQL(insertLogQuery);

                        MessageBox.Show("Added to Cart!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        parentForm?.LoadData();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add to cart. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void frmViewBuilt_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.LoadData();
        }
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewBuilt_Load(object sender, EventArgs e)
        {
            txtbikename.Text = viewbikename;
            txtprice.Text = viewbikeprice;
            txtbranch.Text = viewbikebranch;
            txtbikespecs.Text = viewbikespecs;
            txtstatus.Text = viewsalestatus;

            if (txtstatus.Text == "SOLD" || txtstatus.Text.ToUpper() == "BEING PURCHASED")
            {
                btnaddtocart.Visible = false;
            }

            if (usertype == "STAFF" || usertype == "MANAGER" || usertype == "OWNER")
            {
                btnaddtocart.Visible = false;
            }

            try
            {
                string imageFolder = @"C:\xampp\htdocs\ict127-CS2A-2024\Built\BUILT IMAGE\";
                string[] extensions = { ".jpg", ".jpeg", ".png" };
                string bikeName = txtbikename.Text;

                bool imageFound = false;

                foreach (string ext in extensions)
                {
                    string imagePath = Path.Combine(imageFolder, bikeName + ext);
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


    }
}
