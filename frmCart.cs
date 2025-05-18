using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmCart : Form
    {
        private string username, contact, usertype;
        int row;
        Class1 cart = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        public frmCart(string username, string contact, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;

            if (usertype.ToUpper() == "STAFF" || usertype.ToUpper() == "MANAGER" || usertype.ToUpper() == "OWNER")
            {
                this.Text = "Purchase";
            }
            else
            {
                this.Text = "My Cart";
            }
        }

        private void btnview_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string viewname = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();  // name
                string viewbranch = dataGridView1.SelectedRows[0].Cells[1].Value.ToString(); // branch
                string viewprice = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();  // price
                string viewtype = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();   // type
                string viewstatus = dataGridView1.SelectedRows[0].Cells[4].Value.ToString(); // status
                string viewquantity = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();  // quantity
                string viewdateadded = dataGridView1.SelectedRows[0].Cells[6].Value.ToString(); // DateAdded

                DataTable dt = cart.GetData($"SELECT specs FROM tblproduct WHERE name = '{viewname}'");
                string viewspecs = dt.Rows.Count > 0 ? dt.Rows[0]["specs"].ToString() : "No specs available";

                Form newForm = null;  // Initialize newForm

                if (viewtype.ToUpper() == "BUILT")
                {
                    newForm = new frmCartBuilt(username, contact, usertype, viewname, viewprice, viewbranch, viewstatus, viewquantity, viewdateadded, viewspecs);
                }
                else if (viewtype.ToUpper() == "PARTS")
                {
                    newForm = new frmCartParts(username, contact, usertype, viewname, viewbranch, viewprice, viewstatus, viewquantity, viewdateadded, viewspecs);
                }
                else
                {
                    MessageBox.Show("Unknown product type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (newForm != null)
                {
                    newForm.Owner = this;
                    newForm.ShowDialog();

                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a row to view details.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string viewbuyer = dataGridView1.SelectedRows[0].Cells["buyer"].Value.ToString();
                string viewname = dataGridView1.SelectedRows[0].Cells["name"].Value.ToString();
                string viewbranch = dataGridView1.SelectedRows[0].Cells["branch"].Value.ToString();
                string viewprice = dataGridView1.SelectedRows[0].Cells["price"].Value.ToString();
                string viewcontact = dataGridView1.SelectedRows[0].Cells["contact"].Value.ToString();
                string viewmethod2 = dataGridView1.SelectedRows[0].Cells["method2"].Value.ToString();
                string viewstatus = dataGridView1.SelectedRows[0].Cells["status"].Value.ToString();
                string viewpickupdropoff = dataGridView1.SelectedRows[0].Cells["pickupdropoff"].Value.ToString();
                string viewquantity = dataGridView1.SelectedRows[0].Cells["quantity"].Value.ToString();
                string viewcode = dataGridView1.SelectedRows[0].Cells["code"].Value.ToString();
                string viewdateadded = dataGridView1.SelectedRows[0].Cells["DateAdded"].Value.ToString();
                string viewdatepurchased = dataGridView1.SelectedRows[0].Cells["DatePurchased"].Value.ToString();

                DataTable dtProduct = cart.GetData($"SELECT specs FROM tblproduct WHERE name = '{viewname}'");
                string viewspecs = dtProduct.Rows.Count > 0 ? dtProduct.Rows[0]["specs"].ToString() : "No specs available";

                DataTable dtCart = cart.GetData($"SELECT type FROM tblcart WHERE code = '{viewcode}'");
                if (dtCart.Rows.Count > 0)
                {
                    string realType = dtCart.Rows[0]["type"].ToString().ToUpper();

                    if (realType == "BUILT")
                    {
                        frmUpdateCartBuilt updatecartbuiltform = new frmUpdateCartBuilt(viewbuyer, viewname, viewbranch, viewprice, realType, viewcontact, viewmethod2, viewstatus, viewpickupdropoff, viewquantity, viewcode, viewdateadded, viewdatepurchased, viewspecs);
                        updatecartbuiltform.Show();
                    }
                    else if (realType == "PARTS")
                    {
/*                        frmUpdateCartPart updatecartpartform = new frmUpdateCartPart(viewbuyer, viewname, viewbranch, viewprice, realType, viewcontact, viewmethod2, viewstatus, viewquantity, viewcode, viewdateadded, viewdatepurchased, viewspecs);
                        updatecartpartform.Show();*/
                    }
                    else
                    {
                        MessageBox.Show($"Unsupported type '{realType}' detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"No cart item found for code '{viewcode}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to view details.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        public void LoadData()
        {
            try
            {
                DataTable dt;
                string query = "";

                if (usertype.ToUpper() == "CUSTOMER")
                {
                    query = $"SELECT name, branch, price, type, status, quantity, code, DatePurchased " +
                            $"FROM tblcart WHERE buyer = '{username}' ORDER BY DateAdded DESC";
                }
                else if (usertype.ToUpper() == "OWNER" || usertype.ToUpper() == "STAFF" || usertype.ToUpper() == "MANAGER")
                {
                    query = $"SELECT buyer, name, specs, branch, price, type, contact, method2, status, pickupdropoff, quantity, code, DateAdded, DatePurchased " +
                            $"FROM tblcart ORDER BY DatePurchased DESC";
                }
                else
                {
                    MessageBox.Show("Invalid user type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Console.WriteLine("Executing Query: " + query);

                dt = cart.GetData(query);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on cart load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frmCart_Load(object sender, EventArgs e)
        {

            LoadData();

            if (this.Tag is Dictionary<string, string> cartData)
            {
                string viewname = cartData["name"];
                string viewtype = cartData["type"];
                string viewspecs = cartData["specs"];
                string quantity = cartData["quantity"];
                string Viewbuyer = cartData["buyer"];
                string viewmethod2 = cartData["method2"];
                string viewbranch = cartData["branch"];
                string viewstatus = cartData["status"];
                string viewcode = cartData["code"];
                string viewprice = cartData["price"];
                string viewdatepurchased = cartData["DatePurchased"];
            }
            if (usertype == "CUSTOMER" || usertype == "STAFF")
            {
                btnupdate.Hide();
                btndelete.Hide();
            }
        }


    }
}
