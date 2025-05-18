using System;
using System.Data;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmAddAccount : Form
    {
        Class1 newaccount = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        string username;
        int errorcount;
        private frmUser parentForm;

        public frmAddAccount(string username, frmUser parentForm)
        {
            InitializeComponent();
            this.username = username;
            this.parentForm = parentForm;
        }

        public void validateForm()
        {
            errorcount = 0;
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(txtusername.Text))
            {
                errorProvider1.SetError(txtusername, "Input is empty");
                errorcount++;
            }

            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Input is empty");
                errorcount++;
            }

            if (cmbusertype.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbusertype, "Select usertype");
                errorcount++;
            }

            if (txtcontact.Enabled == true && string.IsNullOrEmpty(txtcontact.Text))
            {
                errorProvider1.SetError(txtcontact, "Input is empty");
                errorcount++;
            }

            try
            {
                DataTable dt = newaccount.GetData("SELECT * FROM tbluser WHERE username = '" + txtusername.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtusername, "Username is already in use.");
                    errorcount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing username", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newaccount.executeSQL("INSERT INTO tbluser (username, password, usertype, status, email, birthdate, contact, DateCreated, CreatedBy) " +
                            "VALUES ('" + txtusername.Text + "', '" + txtpassword.Text + "', '" + cmbusertype.Text.ToUpper() + "', 'ACTIVE', '', '', '" + txtcontact.Text + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + username + "')");

                        if (newaccount.rowAffected > 0)
                        {
                            newaccount.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                                DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'Add', 'Accounts Management', '" + txtusername.Text + "', '" + username + "')");

                            MessageBox.Show("Account Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            parentForm.LoadData(); // Refresh frmUser
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmAddAccount_Load(object sender, EventArgs e)
        {
            txtcontact.Enabled = false;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbusertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbusertype.SelectedIndex == 0)
            {
                txtcontact.Enabled = true;
            }
            else
            {
                txtcontact.Enabled = false;
                txtcontact.Clear();
            }
        }
    }
}
