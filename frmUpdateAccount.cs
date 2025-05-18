using System;
using System.Data;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmUpdateAccount : Form
    {
        Class1 updateaccount = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        string username, editusername, editpassword, edittype, editstatus;
        int errorcount;
        private frmUser parentForm;

        public frmUpdateAccount(string username, string editusername, string editpassword, string edittype, string editstatus, frmUser parentForm)
        {
            InitializeComponent();
            this.username = username;
            this.editusername = editusername;
            this.editpassword = editpassword;
            this.edittype = edittype;
            this.editstatus = editstatus;
            this.parentForm = parentForm;
        }

        private void validateForm()
        {
            errorcount = 0;
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Input is empty");
                errorcount++;
            }

            if (txtpassword.TextLength < 6)
            {
                errorProvider1.SetError(txtpassword, "Password should be at least 6 characters");
                errorcount++;
            }

            if (cmbusertype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbusertype, "Select user type");
                errorcount++;
            }

            if (cmbstatus.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbstatus, "Select account status");
                errorcount++;
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updateaccount.executeSQL("UPDATE tbluser SET password = '" + txtpassword.Text + "', usertype = '" + cmbusertype.Text.ToUpper() + "', status = '" + cmbstatus.Text.ToUpper() + "' WHERE username = '" + txtusername.Text + "'");

                        if (updateaccount.rowAffected > 0)
                        {
                            updateaccount.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                                DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'Update', 'Accounts Management', '" + txtusername.Text + "', '" + username + "')");

                            MessageBox.Show("Account Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            parentForm.LoadData(); // Refresh frmUser
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmUpdateAccount_Load(object sender, EventArgs e)
        {
            txtusername.Text = editusername;
            txtpassword.Text = editpassword;

            cmbusertype.SelectedIndex = cmbusertype.Items.IndexOf(edittype.ToUpper());
            cmbstatus.SelectedIndex = (editstatus.ToUpper() == "ACTIVE") ? 0 : 1;
        }
    }
}
