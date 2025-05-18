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
    public partial class frmHistoryParts : Form
    {
        Class1 HistoryParts = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        private string viewname, viewtype, buyer, contact, method2, branch, price, code, datePurchased, dateReceived;
        public frmHistoryParts(string viewname, string viewtype, string buyer, string contact, string method2, string branch, string price, string code, string datePurchased, string dateReceived)
        {
            InitializeComponent();
            this.viewname = viewname;
            this.viewtype = viewtype;
            this.buyer = buyer;
            this.contact = contact;
            this.method2 = method2;
            this.branch = branch;
            this.price = price;
            this.code = code;
            this.datePurchased = datePurchased;
            this.dateReceived = dateReceived;
        }
        private void frmHistoryParts_Load(object sender, EventArgs e)
        {
            txtpartname.Text = viewname;
            txtbuyer.Text = buyer;
            txtcontact.Text = contact;
            txtmethod2.Text = method2;
            txtbranch.Text = branch;
            txtprice.Text = price;
            txtpurchased.Text = datePurchased;
            txtrecieved.Text = dateReceived;
        }
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
