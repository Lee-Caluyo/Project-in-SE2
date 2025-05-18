using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmUpdateCartBuilt : Form
    {
        int errorcount, servicefee, totalprice;
        string viewbuyer, viewname, viewbranch, viewprice, viewtype, viewcontact, viewmethod2, viewstatus, viewpickupdropoff, viewquantity, viewcode, viewdateadded, viewdatepurchased, viewspecs;
        private frmCart parentForm;
        Class1 updatecartbuilt = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate the status based on cmbstatus selection
                string status = (cmbstatus.SelectedItem.ToString() == "Received") ? "PURCHASED" : cmbstatus.SelectedItem.ToString();

                // Generate the branch code based on the branch entered
                string branchCode = "";
                if (txtbranch.Text == "ARELLANO EXTENSION STREET")
                {
                    branchCode = "AES - " + DateTime.Now.ToString("MM/dd/yyyy/HH/mm/ss/fff");
                }
                else if (txtbranch.Text == "ESTRADA STREET")
                {
                    branchCode = "ES - " + DateTime.Now.ToString("MM/dd/yyyy/HH/mm/ss/fff");
                }

                // Now use the generated branchCode or viewcode (if the viewcode is the unique identifier you want to use for the update)
                string updateQuery = $"UPDATE tblcart SET price = '{txtprice.Text}', status = '{status}', " +
                                     $"pickupdropoff = '{cmbstatus.SelectedItem.ToString()}', code = '{branchCode}' " +
                                     $"WHERE code = '{viewcode}'"; // Use viewcode as the identifier for the record to update

                // Execute the query using your executeSQL method
                updatecartbuilt.executeSQL(updateQuery);

                // Notify user of success
                MessageBox.Show("Cart updated successfully.");

                // Refresh data in frmCart after the update
                if (this.Owner is frmCart parentForm)
                {
                    parentForm.LoadData(); // Refresh data in frmCart
                }

                // Close the form after completion
                this.Close();
            }
            catch (Exception ex)
            {
                // Handle any errors during the update process
                MessageBox.Show($"Error updating cart: {ex.Message}");
            }
        }


        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public frmUpdateCartBuilt(string viewbuyer, string viewname, string viewbranch, string viewprice, string viewtype, string viewcontact, string viewmethod2, string viewstatus, string viewpickupdropoff, string viewquantity, string viewcode, string viewdateadded, string viewdatepurchased, string viewspecs)
        {
            InitializeComponent();
            this.viewbuyer = viewbuyer;
            this.viewname = viewname;
            this.viewbranch = viewbranch;
            this.viewprice = viewprice;
            this.viewtype = viewtype;
            this.viewcontact = viewcontact;
            this.viewmethod2 = viewmethod2;
            this.viewstatus = viewstatus;
            this.viewpickupdropoff = viewpickupdropoff;
            this.viewquantity = viewquantity;
            this.viewcode = viewcode;
            this.viewdateadded = viewdateadded;
            this.viewdatepurchased = viewdatepurchased;
            this.viewspecs = viewspecs;
        }
        private void frmUpdateCartBuilt_Load(object sender, EventArgs e)
        {
            txtname.Text = viewname;
            txtbranch.Text = viewbranch;
            txtbuyer.Text = viewbuyer;
            txtmethod2.Text = viewmethod2;
            txtspecs.Text = viewspecs;

            cmbstatus.Items.Clear();
            cmbstatus.Items.Add("--Select Status--");

            // Convert price to integer
            int.TryParse(viewprice, out int basePrice);
            servicefee = viewmethod2.Equals("DELIVERY", StringComparison.OrdinalIgnoreCase) ? 50 : 0;
            totalprice = basePrice + servicefee;

            // Add status options
            if (servicefee > 0)
                cmbstatus.Items.Add("Delivering");
            else
                cmbstatus.Items.Add("Picking Up");

            cmbstatus.Items.Add("Recieved");

            // Set price with fee
            txtprice.Text = totalprice.ToString();
            cmbstatus.SelectedIndex = 0;
        }

    }
}
