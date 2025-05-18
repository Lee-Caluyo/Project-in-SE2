using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmMain : Form
    {
        private string username, contact, usertype;
        private Form activeForm = null;

        public frmMain(string username, string usertype, string contact)
        {
            InitializeComponent();
            this.username = username;
            this.usertype = usertype;
            this.contact = contact;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Username: " + username;
            toolStripStatusLabel2.Text = "Usertype: " + usertype;

            // Set toolstrip menu item names based on usertype
            if (usertype == "STAFF" || usertype == "MANAGER" || usertype == "OWNER")
            {
                cartToolStripMenuItem.Text = "Purchase";
            }
            else if (usertype == "CUSTOMER")
            {
                cartToolStripMenuItem.Text = "Cart";
            }

            // Configure visibility
            if (usertype == "CUSTOMER")
            {
                builtToolStripMenuItem.Visible = true;
                partsToolStripMenuItem.Visible = true;
                repairToolStripMenuItem.Visible = true;
                userToolStripMenuItem.Visible = false;
                cartToolStripMenuItem.Visible = true;
                profileToolStripMenuItem.Visible = true;
                historyToolStripMenuItem.Visible = false;
            }
            else if (usertype == "STAFF")
            {
                builtToolStripMenuItem.Visible = true;
                partsToolStripMenuItem.Visible = true;
                repairToolStripMenuItem.Visible = true;
                userToolStripMenuItem.Visible = false;
                cartToolStripMenuItem.Visible = true;
                profileToolStripMenuItem.Visible = false;
                historyToolStripMenuItem.Visible = true;
            }
            else if (usertype == "OWNER" || usertype == "MANAGER")
            {
                builtToolStripMenuItem.Visible = true;
                partsToolStripMenuItem.Visible = true;
                repairToolStripMenuItem.Visible = true;
                userToolStripMenuItem.Visible = true;
                cartToolStripMenuItem.Visible = true;
                profileToolStripMenuItem.Visible = false;
                historyToolStripMenuItem.Visible = true;
            }
        }

        private void OpenForm(Form newForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = newForm;
            activeForm.MdiParent = this;
            activeForm.Show();
        }

        private void cartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usertype == "STAFF" || usertype == "MANAGER" || usertype == "OWNER")
            {
                cartToolStripMenuItem.Text = "Purchase";
            }
            else if (usertype == "CUSTOMER")
            {
                cartToolStripMenuItem.Text = "Cart";
            }

            menuStrip1.Refresh();
            OpenForm(new frmCart(username, contact, usertype));
        }

        private void builtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmBuilt(username, contact, usertype));
        }

        private void partsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmParts(username, contact, usertype));
        }

        private void repairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmRepair(username, contact, usertype));
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmUser(username, contact, usertype));
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmHistory(username, contact, usertype));
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmProfile());
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
        }
    }
}
