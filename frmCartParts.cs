using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmCartParts : Form
    {
        private string name, contact, usertype, productName, branch, price, status, quantity, dateAdded, specs;
        public frmCartParts(string name, string contact, string usertype, string productName, string branch, string price, string status, string quantity, string dateAdded, string specs)
        {
            InitializeComponent();
            this.name = name;
            this.contact = contact;
            this.usertype = usertype;
            this.productName = productName;
            this.branch = branch;
            this.price = price;
            this.status = status;
            this.quantity = quantity;
            this.dateAdded = dateAdded;
            this.specs = specs;
        }

        private void frmCartParts_Load(object sender, EventArgs e)
        {
            txtname.Text = productName;
            txtprice.Text = price;
            txtquantity.Text = quantity;
        }
    }
}
