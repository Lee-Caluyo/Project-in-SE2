using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmCartBuilt : Form
    {
        Class1 cartbuilt = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        private string name, contact, usertype, productName, price, branch, status, method, dateAdded, specs;

        private void btnpurchase_Click(object sender, EventArgs e)
        {

        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to remove this from your cart?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    string deleteQuery = $"DELETE FROM tblcart WHERE name = '{productName}' AND username = '{name}'";

                    try
                    {
                        cartbuilt.GetData(deleteQuery);
                    }
                    catch (Exception)
                    {

                    }

                    MessageBox.Show("Item removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Owner is frmCart cartForm)
                    {
                        cartForm.LoadData();
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error while removing item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public frmCartBuilt(string name, string contact, string usertype, string productName, string price, string branch, string status, string method, string dateAdded, string specs)
        {
            InitializeComponent();
            this.name = name;
            this.contact = contact;
            this.usertype = usertype;
            this.productName = productName;
            this.price = price;
            this.branch = branch;
            this.status = status;
            this.method = method;
            this.dateAdded = dateAdded;
            this.specs = specs;
        }

        private void frmCartBuilt_Load(object sender, EventArgs e)
        {
            txtname.Text = productName;
            txtprice.Text = price;
            txtbranch.Text = branch;
            txtstatus.Text = status;
            txtspecs.Text = specs;
            if (txtstatus.Text == "IN CART")
            {
                txtmethod2.Hide();
                lblmethod2.Hide();
            }
            else
            {
                txtmethod2.Text = method;
                lblmethod2.Show();
            }
        }
    }
}
