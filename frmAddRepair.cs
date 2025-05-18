using System;
using System.Data;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmAddRepair : Form
    {
        private string username, contact, usertype;
        int errorcount;
        private string repairCode;

        Class1 addrepair = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        public frmAddRepair(string username, string contact, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;
        }

        private void frmAddRepair_Load(object sender, EventArgs e)
        {
            // Fetch the contact from tbluser based on the username
            string query = $"SELECT contact FROM tbluser WHERE username = '{username}'";
            DataTable dt = addrepair.GetData(query);

            if (dt.Rows.Count > 0)
            {
                contact = dt.Rows[0]["contact"].ToString();
            }

            txtcontact.Text = contact;
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            cmbbranch.SelectedIndex = -1;
            cmbmethod1.SelectedIndex = -1;
            cmbmethod2.SelectedIndex = -1;
            txtaddress.Enabled = false;
            txtaddress.Clear();
        }

        private void cmbmethod1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAddressEnabled();
        }

        private void cmbmethod2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAddressEnabled();
        }

        private void UpdateAddressEnabled()
        {
            bool enable = false;

            if (cmbmethod1.SelectedIndex == 0) enable = true;
            if (cmbmethod1.SelectedIndex == 1 && cmbmethod2.SelectedIndex == 1) enable = true;

            txtaddress.Enabled = enable;
            if (!enable) txtaddress.Clear();
        }

        private void btndone_Click(object sender, EventArgs e)
        {
            if (!validateForm()) return;

            DialogResult dr = MessageBox.Show("Are you sure you want to add this request?",
                                              "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // Determine bikerecieving and bikestatus based on method1
                string method1 = cmbmethod1.SelectedItem.ToString();
                string bikerecieving = method1 == "Pick Up" ? "Picking Up" : "Waiting";
                string bikestatus = method1 == "Drop Off" ? "WAITING FOR ARRIVAL" :
                                    method1 == "Pick Up" ? "WAITING FOR ARRIVAL" : "UNKNOWN";

                if (string.IsNullOrEmpty(repairCode))
                {
                    repairCode = GenerateUniqueCode();
                }

                string insertQuery = $@"
                    INSERT INTO tblrepair 
                    (username, concern, branch, method1, method2, code, mechanic1, mechanic2, bikerecieving, bikestatus, repaircomplete, repairedstatus, address, contact, price, DateRequested, DateComplete, DateDelivered, DateRecieved)
                    VALUES ('{username}', '{txtconcern.Text}', '{cmbbranch.SelectedItem}', '{cmbmethod1.SelectedItem}', '{cmbmethod2.SelectedItem}', '{repairCode}', ' ', ' ', '{bikerecieving}', '{bikestatus}', ' ', ' ', '{txtaddress.Text}', '{contact}', ' ', '{DateTime.Now:yyyy-MM-dd}', ' ', ' ', ' ')";

                try
                {
                    addrepair.executeSQL(insertQuery);
                    MessageBox.Show("Repair request added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private bool validateForm()
        {
            errorProvider1.Clear();
            errorcount = 0;
            bool valid = true;

            if (cmbbranch.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbbranch, "Select a branch");
                valid = false;
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtconcern.Text))
            {
                errorProvider1.SetError(txtconcern, "Input is empty");
                valid = false;
                errorcount++;
            }
            if (cmbmethod1.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbmethod1, "Select a method");
                valid = false;
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtcontact.Text))
            {
                errorProvider1.SetError(txtcontact, "Contact is empty");
                valid = false;
                errorcount++;
            }
            if (cmbmethod2.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbmethod2, "Select a method");
                valid = false;
                errorcount++;
            }
            if (txtaddress.Enabled && string.IsNullOrEmpty(txtaddress.Text))
            {
                errorProvider1.SetError(txtaddress, "Address is empty");
                valid = false;
                errorcount++;
            }

            return valid;
        }

        private string GenerateUniqueCode()
        {
            return DateTime.Now.ToString("R - " + "MMddyyyyHHmmssfff");
        }
    }
}
