using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace CS311A2024_DATABASE
{
    public partial class frmHistoryBuilt : Form
    {
        private string viewname, viewtype, buyer, contact, method2, branch, price, code, datePurchased, dateReceived, specs;
        Class1 HistoryBuilt = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        public frmHistoryBuilt(string viewname, string viewtype, string buyer, string contact, string method2, string branch, string price, string code, string datePurchased, string dateReceived, string specs)
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
            this.specs = specs;
        }
        private void frmHistoryBuilt_Load(object sender, EventArgs e)
        {
            txtbikename.Text = viewname;
            txtbuyer.Text = buyer;
            txtbranch.Text = branch;
            txtpurchased.Text = price;
            txtmethod2.Text = method2;
            txtcode.Text = code;
            txtpurchased.Text = datePurchased;
            txtrecieved.Text = dateReceived;
        }
    }
}
