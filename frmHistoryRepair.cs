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
    public partial class frmHistoryRepair : Form
    {
        private string customer, concern, mechanic1, mechanic2, code, contact, address, branch, method1, method2, price, dateRequested, dateComplete, dateReceived;
        Class1 HistoryRepair = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        public frmHistoryRepair(string customer, string concern, string mechanic1, string mechanic2, string code, string contact, string address, string branch, string method1, string method2, string price, string dateRequested, string dateComplete, string dateReceived)
        {
            InitializeComponent();
            this.customer = customer;
            this.concern = concern;
            this.mechanic1 = mechanic1;
            this.mechanic2 = mechanic2;
            this.code = code;
            this.address = address;
            this.branch = branch;
            this.method1 = method1;
            this.method2 = method2;
            this.price = price;
            this.dateRequested = dateRequested;
            this.dateComplete = dateComplete;
            this.dateReceived = dateReceived;
        }

        private void frmHistoryRepair_Load(object sender, EventArgs e)
        {
            txtbranch.Text = branch;
            txtconcern.Text = concern;
            txtprice.Text = price;
            txtcustomer.Text = customer;
            txtmethod1.Text = method1;
            txtcontact.Text = contact;
            txtmechanic1.Text = mechanic1;
            txtmechanic2.Text = mechanic2;
            txtaddress.Text = address;
            txtcode.Text = code;
            txtmethod2.Text = method2;
            txtrequested.Text = dateRequested;
            txtcompleted.Text = dateComplete;
            txtrecieved.Text = dateReceived;
        }
    }
}
