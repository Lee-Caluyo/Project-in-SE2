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
    public partial class frmParts : Form
    {
        private string username, contact, usertype;
        int row;
        private frmParts parentForm;
        Class1 parts = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        public frmParts(string username, string contact, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;
        }

        public void LoadData()
        {
            try
            {
                DataTable dt = parts.GetData("SELECT name, branch, stock, price FROM tblproduct WHERE type = 'PARTS' ORDER BY name");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading products", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnview_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string viewpartname = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string viewpartbranch = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string viewpartstock = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string viewpartprice = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

                DataTable dt = parts.GetData($"SELECT specs FROM tblproduct WHERE name = '{viewpartname}'");
                string viewpartspecs = dt.Rows.Count > 0 ? dt.Rows[0]["specs"].ToString() : "No specs available";

                frmViewParts newviewpartsform = new frmViewParts(this, username, contact, usertype, viewpartname, viewpartbranch, viewpartstock, viewpartprice);
                newviewpartsform.ShowDialog();

                LoadData();
            }
        }
        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT name, branch, stock, price FROM tblproduct " +
                               "WHERE type = 'PARTS' " +
                               "AND (name LIKE '%" + txtsearch.Text + "%' " +
                               "OR branch LIKE '%" + txtsearch.Text + "%') " +
                               "ORDER BY name";

                DataTable dt = parts.GetData(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddParts addpartsform = new frmAddParts(username, this);
            addpartsform.Show();
            LoadData();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int rowIndex = dataGridView1.CurrentRow.Index;
                string editname = dataGridView1.Rows[rowIndex].Cells["name"].Value?.ToString() ?? "";
                string editbranch = dataGridView1.Rows[rowIndex].Cells["branch"].Value?.ToString() ?? "";
                string editstock = dataGridView1.Rows[rowIndex].Cells["stock"].Value?.ToString() ?? "";
                string editprice = dataGridView1.Rows[rowIndex].Cells["price"].Value?.ToString() ?? "";

                frmUpdateParts updatepartform = new frmUpdateParts(username, editname, editbranch, editstock, editprice, this);
                updatepartform.Show();
            }
            else
            {
                MessageBox.Show("Please select a row first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int row = dataGridView1.CurrentRow.Index;
                string selectedPart = dataGridView1.Rows[row].Cells["name"].Value.ToString();
                string selectedBranch = dataGridView1.Rows[row].Cells["branch"].Value.ToString();

                DialogResult dr = MessageBox.Show($"Are you sure you want to delete the part? '{selectedPart}'?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        parts.executeSQL("DELETE FROM tblproduct WHERE name = '" + selectedPart + "' AND branch = '" + selectedBranch + "'");

                        if (parts.rowAffected > 0)
                        {
                            parts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                                DateTime.Now.ToShortDateString() + "', '" +
                                DateTime.Now.ToShortTimeString() + "', 'Delete', 'Bike Management', '" +
                                selectedPart + "', '" + username + "')");

                            MessageBox.Show("Part Deleted Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Deletion failed. The part may not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

        private void frmParts_Load(object sender, EventArgs e)
        {
            LoadData();
            if (usertype == "CUSTOMER" || usertype == "STAFF")
            {
                btnadd.Hide();
                btnupdate.Hide();
                btndelete.Hide();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    row = e.RowIndex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on datagrid cellclick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
