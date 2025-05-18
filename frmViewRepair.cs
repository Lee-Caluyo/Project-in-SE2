using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmViewRepair : Form
    {
        private frmRepair parentForm;
        private string username, contact, usertype, viewusername, viewcontact, viewconcern, viewbranch, viewmethod1, viewmethod2, viewcode, viewmechanic1, viewmechanic2, viewbikerecieving, viewbikestatus, viewpickupdropoff, viewaddress, viewservicefee, viewrepairfee, viewprice, viewdaterequested, viewdatecomplete, viewdatedelivered, viewdaterecieved;

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public frmViewRepair(frmRepair parent, string username, string contact, string usertype, string viewusername, string viewcontact, string viewconcern, string viewbranch, string viewmethod1, string viewmethod2, string viewcode, string viewmechanic1, string viewmechanic2, string viewbikerecieving, string viewbikestatus, string viewpickupdropoff, string viewaddress, string viewservicefee, string viewrepairfee, string viewprice, string viewdaterequested, string viewdatecomplete, string viewdatedelivered, string viewdaterecieved)
        {
            InitializeComponent();
            this.parentForm = parent;
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;
            this.viewusername = viewusername;
            this.viewcontact = viewcontact;
            this.viewconcern = viewconcern;
            this.viewbranch = viewbranch;
            this.viewmethod1 = viewmethod1;
            this.viewmethod2 = viewmethod2;
            this.viewcode = viewcode;
            this.viewmechanic1 = viewmechanic1;
            this.viewmechanic2 = viewmechanic2;
            this.viewbikerecieving = viewbikerecieving;
            this.viewbikestatus = viewbikestatus;
            this.viewaddress = viewaddress;
            this.viewservicefee = viewservicefee;
            this.viewrepairfee = viewrepairfee;
            this.viewprice = viewprice;
            this.viewdaterequested = viewdaterequested;
            this.viewdatecomplete = viewdatecomplete;
            this.viewdatedelivered = viewdatedelivered;
            this.viewdaterecieved = viewdaterecieved;
        }

        private void frmViewRepair_Load(object sender, EventArgs e)
        {
            txtuser.Text = viewusername;
            txtcontact.Text = viewcontact;
            txtbranch.Text = viewbranch;
            txtconcern.Text = viewconcern;
            txtmethod1.Text = viewmethod1;
            txtaddress.Text = viewaddress;
            txtmethod2.Text = viewmethod2;
            txtmechanic1.Text = viewmechanic1;
            txtmechanic2.Text = viewmechanic2;
            txtstatus.Text = viewbikestatus;
            txtbikerecieving.Text = viewbikerecieving;
            txtbikestatus.Text = viewbikestatus;
            txtservicefee.Text = viewservicefee;
            txtrepairfee.Text = viewrepairfee;
            txtprice.Text = viewprice;
            txtmechanic1.Text = viewmechanic1;
            txtmechanic2.Text = viewmechanic2;
            txtdaterequested.Text = viewdaterequested;
            txtdatedelivered.Text = viewdatedelivered;
            txtdatecomplete.Text = viewdatecomplete;
            txtdaterecieved.Text = viewdaterecieved;
        }


        private void frmViewBuilt_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.LoadData();
        }
    }
}
