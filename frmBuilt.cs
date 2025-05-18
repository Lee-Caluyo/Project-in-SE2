using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CS311A2024_DATABASE
{
    public partial class frmBuilt : Form
    {
        private string username, contact, usertype;
        int row;
        Class1 built = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        public frmBuilt(string username, string contact, string usertype)
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
                DataTable dt = built.GetData("SELECT name, branch, specs, price, SaleStatus, DateCreated, CreatedBy FROM tblproduct WHERE type = 'BUILT' ORDER BY name");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading products", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT name, branch, specs, price, SaleStatus, DateCreated, CreatedBy FROM tblproduct " +
                               "WHERE type = 'BUILT' " +
                               "AND (name LIKE '%" + txtsearch.Text + "%' " +
                               "OR branch LIKE '%" + txtsearch.Text + "%') " +
                               "ORDER BY name";

                DataTable dt = built.GetData(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on search", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddBuilt addbuiltform = new frmAddBuilt(username, this);
            addbuiltform.Show();
            LoadData();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int rowIndex = dataGridView1.CurrentRow.Index;
                string editname = dataGridView1.Rows[rowIndex].Cells["name"].Value?.ToString() ?? "";
                string editbranch = dataGridView1.Rows[rowIndex].Cells["branch"].Value?.ToString() ?? "";
                string editspecs = dataGridView1.Rows[rowIndex].Cells["specs"].Value?.ToString() ?? "";
                string editprice = dataGridView1.Rows[rowIndex].Cells["price"].Value?.ToString() ?? "";
                string editsalestatus = dataGridView1.Rows[rowIndex].Cells["SaleStatus"].Value?.ToString() ?? "";

                frmUpdateBuilt updatebuiltform = new frmUpdateBuilt(username, editname, editbranch, editspecs, editprice, editsalestatus, this);
                updatebuiltform.Show();
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
                string selectedBike = dataGridView1.Rows[row].Cells["name"].Value.ToString();
                string selectedBranch = dataGridView1.Rows[row].Cells["branch"].Value.ToString();

                DialogResult dr = MessageBox.Show($"Are you sure you want to delete the bike '{selectedBike}'?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        // Delete the bike from the database
                        built.executeSQL("DELETE FROM tblproduct WHERE name = '" + selectedBike + "' AND branch = '" + selectedBranch + "'");

                        if (built.rowAffected > 0)
                        {
                            // Delete associated images if deletion was successful
                            string imageFolderPath = @"C:\xampp\htdocs\ict127-CS2A-2024\Built\BUILT IMAGE\";
                            string[] extensions = { ".jpg", ".jpeg", ".png" };

                            foreach (string ext in extensions)
                            {
                                string imagePath = System.IO.Path.Combine(imageFolderPath, selectedBike + ext);
                                try
                                {
                                    if (System.IO.File.Exists(imagePath))
                                    {
                                        System.IO.File.Delete(imagePath);
                                    }
                                }
                                catch
                                {
                                    // Ignore errors during image deletion
                                }
                            }

                            // Log the deletion action
                            built.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                                DateTime.Now.ToShortDateString() + "', '" +
                                DateTime.Now.ToShortTimeString() + "', 'Delete', 'Bike Management', '" +
                                selectedBike + "', '" + username + "')");

                            MessageBox.Show("Bike and associated images deleted successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Deletion failed. The bike may not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnview_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string viewbikename = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string viewbikebranch = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string viewbikeprice = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string viewsalestatus = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();

                DataTable dt = built.GetData($"SELECT specs FROM tblproduct WHERE name = '{viewbikename}'");
                string viewbikespecs = dt.Rows.Count > 0 ? dt.Rows[0]["specs"].ToString() : "No specs available";

                frmViewBuilt newviewbuiltform = new frmViewBuilt(this, username, usertype, contact, viewbikename, viewbikebranch, viewbikespecs, viewbikeprice, viewsalestatus);

                newviewbuiltform.ShowDialog();

                LoadData();
            }
        }
        private void frmBuilt_Load(object sender, EventArgs e)
        {
            LoadData();
            if (usertype == "CUSTOMER" || usertype == "STAFF")
            {
                btnadd.Hide();
                btnupdate.Hide();
                btndelete.Hide();
            }
        }
    }
}
