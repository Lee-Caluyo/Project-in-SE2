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
    public partial class frmHistory : Form
    {
        private string username, contact, usertype;
        int row;
        Class1 history = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");
        public frmHistory(string username, string contact, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;
        }

        private void btnrepair_Click(object sender, EventArgs e)
        {
            txtsearch.Clear();
            try
            {
                string query = "SELECT customer, concern, mechanic1, mechanic2, code, address, contact, branch, method1, method2, price, DateRequested, DateComplete, DateRecieved " +
                               "FROM tblrepairhistory ORDER BY DateRequested DESC";
                DataTable dt = history.GetData(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading repair history", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnpurchase_Click(object sender, EventArgs e)
        {
            txtsearch.Clear();  
            try
            {
                string query = "SELECT name, type, buyer, contact, method2, branch, price, code, DatePurchased, DateRecieved, specs " +
                               "FROM tblpurchasehistory ORDER BY DatePurchased DESC";
                DataTable dt = history.GetData(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading purchase history", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            LoadHistory(txtsearch.Text.Trim());
        }

        private void LoadHistory(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Clear the DataGridView when the search box is empty
                dataGridView1.DataSource = null;
                return;
            }

            searchText = searchText.Replace("'", "''"); // escape quotes

            string purchaseQuery = "SELECT name AS 'Name/Customer', type AS 'Type/Concern', buyer AS 'Buyer/Mechanic1', contact, method2, branch, " +
                                   "quantity, price, code, DatePurchased AS 'DateReq/Purch', DateRecieved, specs AS 'Specs/Mechanic2' " +
                                   "FROM tblpurchasehistory " +
                                   $"WHERE name LIKE '%{searchText}%' OR buyer LIKE '%{searchText}%' OR contact LIKE '%{searchText}%' " +
                                   $"OR branch LIKE '%{searchText}%' OR type LIKE '%{searchText}%' " +
                                   "ORDER BY DatePurchased DESC";

            string repairQuery = "SELECT customer AS 'Name/Customer', concern AS 'Type/Concern', mechanic1 AS 'Buyer/Mechanic1', contact, method2, branch, " +
                                 "'' AS quantity, price, code, DateRequested AS 'DateReq/Purch', DateRecieved, mechanic2 AS 'Specs/Mechanic2' " +
                                 "FROM tblrepairhistory " +
                                 $"WHERE customer LIKE '%{searchText}%' OR concern LIKE '%{searchText}%' OR contact LIKE '%{searchText}%' " +
                                 $"OR branch LIKE '%{searchText}%' OR mechanic1 LIKE '%{searchText}%' " +
                                 "ORDER BY DateRequested DESC";

            try
            {
                DataTable dtPurchase = history.GetData(purchaseQuery);
                DataTable dtRepair = history.GetData(repairQuery);

                DataTable combined = dtPurchase.Clone(); // clone structure

                foreach (DataRow row in dtPurchase.Rows)
                    combined.ImportRow(row);

                foreach (DataRow row in dtRepair.Rows)
                    combined.ImportRow(row);

                dataGridView1.DataSource = combined;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while searching", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void btnview_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    // Determine if it's purchase or repair by checking column names
                    bool isPurchase = dataGridView1.Columns.Contains("type");
                    bool isRepair = dataGridView1.Columns.Contains("mechanic1") || dataGridView1.Columns.Contains("code");

                    if (isPurchase)
                    {
                        string viewname = dataGridView1.SelectedRows[0].Cells["name"].Value.ToString();
                        string viewtype = dataGridView1.SelectedRows[0].Cells["type"].Value.ToString();
                        string buyer = dataGridView1.SelectedRows[0].Cells["buyer"].Value.ToString();
                        string contact = dataGridView1.SelectedRows[0].Cells["contact"].Value.ToString();
                        string method2 = dataGridView1.SelectedRows[0].Cells["method2"].Value.ToString();
                        string branch = dataGridView1.SelectedRows[0].Cells["branch"].Value.ToString();
                        string price = dataGridView1.SelectedRows[0].Cells["price"].Value.ToString();
                        string code = dataGridView1.SelectedRows[0].Cells["code"].Value.ToString();
                        string datePurchased = dataGridView1.SelectedRows[0].Cells["DatePurchased"].Value.ToString();
                        string dateReceived = dataGridView1.SelectedRows[0].Cells["DateRecieved"].Value.ToString();
                        string specs = dataGridView1.SelectedRows[0].Cells["specs"].Value.ToString();

                        Form viewForm = null;

                        if (viewtype.ToUpper() == "BUILT")
                        {
                            viewForm = new frmHistoryBuilt(viewname, viewtype, buyer, contact, method2, branch, price, code, datePurchased, dateReceived, specs);
                        }
                        else if (viewtype.ToUpper() == "PARTS")
                        {
                            viewForm = new frmHistoryParts(viewname, viewtype, buyer, contact, method2, branch, price, code, datePurchased, dateReceived);
                        }
                        else
                        {
                            MessageBox.Show("Unknown product type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        viewForm.ShowDialog();
                    }
                    else if (isRepair)
                    {
                        string customer = dataGridView1.SelectedRows[0].Cells["customer"].Value.ToString();
                        string concern = dataGridView1.SelectedRows[0].Cells["concern"].Value.ToString();
                        string mechanic1 = dataGridView1.SelectedRows[0].Cells["mechanic1"].Value.ToString();
                        string mechanic2 = dataGridView1.SelectedRows[0].Cells["mechanic2"].Value.ToString();
                        string code = dataGridView1.SelectedRows[0].Cells["code"].Value.ToString();
                        string address = dataGridView1.SelectedRows[0].Cells["address"].Value.ToString();
                        string contact = dataGridView1.SelectedRows[0].Cells["contact"].Value.ToString();
                        string branch = dataGridView1.SelectedRows[0].Cells["branch"].Value.ToString();
                        string method1 = dataGridView1.SelectedRows[0].Cells["method1"].Value.ToString();
                        string method2 = dataGridView1.SelectedRows[0].Cells["method2"].Value.ToString();
                        string price = dataGridView1.SelectedRows[0].Cells["price"].Value.ToString();
                        string dateRequested = dataGridView1.SelectedRows[0].Cells["DateRequested"].Value.ToString();
                        string dateComplete = dataGridView1.SelectedRows[0].Cells["DateComplete"].Value.ToString();
                        string dateReceived = dataGridView1.SelectedRows[0].Cells["DateRecieved"].Value.ToString();

                        frmHistoryRepair viewRepair = new frmHistoryRepair(customer, concern, mechanic1, mechanic2, code, contact, address, branch, method1, method2, price, dateRequested, dateComplete, dateReceived);
                        viewRepair.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Unknown data type in grid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error viewing history:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to view.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnrefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }
    }
}
