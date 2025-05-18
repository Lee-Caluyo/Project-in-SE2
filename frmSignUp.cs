using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311A2024_DATABASE
{
    public partial class frmSignUp : Form
    {
        public frmSignUp()
        {
            InitializeComponent();
        }
        private int errorcount;
        Class1 newuser = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        private void validateForm()
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
            if (txtpassword.Text.Length <= 3)
            {
                errorProvider1.SetError(txtpassword, "Pasword must be more than 4 characters");
                    errorcount++;
            }
            if (string.IsNullOrEmpty(txtreenter.Text))
            {
                errorProvider1.SetError(txtreenter, "Input is empty");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtcontact.Text))
            {
                errorProvider1.SetError(txtcontact, "Input is empty");
                errorcount++;
            }
            if (txtreenter.Text != txtpassword.Text)
            {
                errorProvider1.SetError(txtreenter, "Password is not the same");
                errorcount++;
            }
            try
            {
                DataTable dt = newuser.GetData("SELECT * FROM tbluser WHERE username = '" + txtusername.Text + "'");
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

        private void btncancel_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
            this.Hide();
        }

        private void btnsignup_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newuser.executeSQL("INSERT INTO tbluser (username, password, usertype, status, email, birthdate, contact, CreatedBy, DateCreated) " +
                            "VALUES ('" + txtusername.Text + "', '" + txtpassword.Text + "', 'CUSTOMER', 'ACTIVE', 'Insert Email', 'Insert Birthdate MM/DD/YYYY', '" + txtcontact.Text + "', 'NONE', '" + DateTime.Now.ToShortDateString() + "')");

                        if (newuser.rowAffected > 0)
                        {
                            newuser.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                "VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'Add', 'Accounts Management', 'CUSTOMER', 'CUSTOMER')");

                            MessageBox.Show("Account Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                            frmLogin frmLogin = new frmLogin();
                            frmLogin.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
