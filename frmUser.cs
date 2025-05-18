using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmUser : Form
    {
        private string username, usertype, contact;
        Class1 user = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        public frmUser(string username, string contact, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddAccount addaccountform = new frmAddAccount(username, this);
            addaccountform.Show();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int rowIndex = dataGridView1.CurrentRow.Index;
                string editusername = dataGridView1.Rows[rowIndex].Cells["username"].Value?.ToString() ?? "";
                string editpassword = dataGridView1.Rows[rowIndex].Cells["password"].Value?.ToString() ?? "";
                string edittype = dataGridView1.Rows[rowIndex].Cells["usertype"].Value?.ToString() ?? "";
                string editstatus = dataGridView1.Rows[rowIndex].Cells["status"].Value?.ToString() ?? "";

                frmUpdateAccount updateaccountform = new frmUpdateAccount(username, editusername, editpassword, edittype, editstatus, this);
                updateaccountform.Show();
            }
            else
            {
                MessageBox.Show("Please select a row first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes && dataGridView1.CurrentRow != null)
            {
                try
                {
                    int row = dataGridView1.CurrentRow.Index;
                    string selectedUser = dataGridView1.Rows[row].Cells["username"].Value.ToString();

                    user.executeSQL("DELETE FROM tbluser WHERE username = '" + selectedUser + "'");

                    if (user.rowAffected > 0)
                    {
                        user.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                            DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'Delete', 'Accounts Management', '" + selectedUser + "', '" + username + "')");

                        MessageBox.Show("Account Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtsearch.Text.Trim().Replace("'", "''"); // Escape single quotes for safety
            string query = "";

            if (usertype == "STAFF")
            {
                query = $"SELECT username, usertype, status, email, birthdate, contact, DateCreated, CreatedBy " +
                        $"FROM tbluser WHERE username LIKE '%{searchText}%' ORDER BY username DESC";
            }
            else if (usertype == "MANAGER" || usertype == "OWNER")
            {
                query = $"SELECT username, password, usertype, status, email, birthdate, contact, DateCreated, CreatedBy " +
                        $"FROM tbluser WHERE username LIKE '%{searchText}%' ORDER BY username DESC";
            }

            try
            {
                DataTable dt = user.GetData(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error search", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void LoadData()
        {
            if (usertype == "STAFF")
            {
                try
                {
                    DataTable dt = user.GetData("SELECT username, usertype, status, email, birthdate, contact, DateCreated, CreatedBy FROM tbluser ORDER BY username DESC");
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error loading users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (usertype == "MANAGER" || usertype == "OWNER")
            {
                try
                {
                    DataTable dt = user.GetData("SELECT username, password, usertype, status, email, birthdate, contact, DateCreated, CreatedBy FROM tbluser ORDER BY username DESC");
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error loading users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
