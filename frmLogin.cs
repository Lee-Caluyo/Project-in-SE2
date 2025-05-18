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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        Class1 login = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        private int errorCount;



        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnlogin.PerformClick();
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                errorProvider1.SetError(txtusername, "Input is empty");
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Input is empty");
            }

            errorCount = 0;
            foreach (Control c in errorProvider1.ContainerControl.Controls)
            {
                if (!(string.IsNullOrEmpty(errorProvider1.GetError(c))))
                {
                    errorCount++;
                }
            }

            if (errorCount == 0)
            {
                try
                {
                    DataTable dt = login.GetData("SELECT * FROM tbluser WHERE username = '" + txtusername.Text + "' AND password = '" + txtpassword.Text + "' AND status = 'ACTIVE'");

                    if (dt.Rows.Count > 0)
                    {
                        string usertype = dt.Rows[0].Field<string>("usertype");
                        string contact = dt.Rows[0].Field<string>("contact");

                        frmMain mainform = new frmMain(txtusername.Text, usertype, contact);
                        mainform.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect account information or account is inactive", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnsignup_Click(object sender, EventArgs e)
        {
            frmSignUp signupform = new frmSignUp();
            signupform.Show();
            this.Hide();
        }
        private void chxpass_CheckedChanged(object sender, EventArgs e)
        {
            txtpassword.UseSystemPasswordChar = false;
            if (chxpass.Checked)
            {
                txtpassword.PasswordChar = '\0';
            }
            else
            {
                txtpassword.PasswordChar = '*';
            }
            txtpassword.Refresh();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            btnsignup.Hide();
        }
    }
}
