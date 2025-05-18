using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public partial class frmUpdateRepair : Form
    {
        Class1 updaterepair = new Class1("127.0.0.1", "cs311a2024", "lee", "12345");

        private int errorCount, servicefee, repairfee, totalprice;
        private frmRepair parentForm;
        private string username, contact, usertype,
                       viewusername, viewcontact, viewconcern, viewbranch,
                       viewmethod1, viewmethod2, viewcode,
                       viewmechanic1, viewmechanic2, viewbikerecieving,
                       viewbikestatus, viewpickupdropoff, viewrepaircomplete, viewrepairedstatus,
                       viewaddress, viewservicefee, viewrepairfee, viewprice, viewdaterequested, viewdatecomplete,
                       viewdatedelivered, viewdaterecieved;

        public frmUpdateRepair(
            frmRepair parent, string username, string contact, string usertype,
            string viewusername, string viewcontact, string viewconcern, string viewbranch,
            string viewmethod1, string viewmethod2, string viewcode,
            string viewmechanic1, string viewmechanic2, string viewbikerecieving,
            string viewbikestatus, string viewpickupdropoff, string viewaddress, string viewservicefee, string viewrepairfee, string viewprice, string viewdaterequested,
            string viewdatecomplete, string viewdatedelivered, string viewdaterecieved)
        {
            InitializeComponent();

            this.parentForm = parent;
            this.username = username;
            this.contact = contact;
            this.usertype = usertype;
            this.viewusername = viewusername;
            this.viewcontact = viewcontact;
            this.viewconcern = viewconcern;
            this.viewbranch = viewbranch;
            this.viewmethod1 = viewmethod1;
            this.viewmethod2 = viewmethod2;
            this.viewcode = viewcode;
            this.viewmechanic1 = viewmechanic1;
            this.viewmechanic2 = viewmechanic2;
            this.viewbikerecieving = viewbikerecieving;
            this.viewbikestatus = viewbikestatus;
            this.viewaddress = viewaddress;
            this.viewservicefee = viewservicefee;
            this.viewrepairfee = viewrepairfee;
            this.viewprice = viewprice;
            this.viewdaterequested = viewdaterequested;
            this.viewdatecomplete = viewdatecomplete;
            this.viewdatedelivered = viewdatedelivered;
            this.viewdaterecieved = viewdaterecieved;
        }
        private void cmbbikestatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbbikestatus.SelectedIndex == 0 || cmbbikestatus.Text == "Select Status")
            {
                txtrepairfee.Enabled = true;
                cmbstatus.Enabled = true;
                txtdaterecieved.Text = "";
            }
            if (cmbbikestatus.SelectedIndex == 1 || cmbbikestatus.Text == "Delivery" || cmbbikestatus.Text == "Picking Up")
            {
                txtrepairfee.Enabled = false;
                cmbstatus.Enabled = false;
                txtdaterecieved.Text = "";
                txtdatedelivered.Text = DateTime.Now.ToString("MM/dd/yyyy");
            }
            if (cmbbikestatus.SelectedIndex == 2 || cmbbikestatus.Text == "Received")
            {
                txtrepairfee.Enabled = false;
                cmbstatus.Enabled = false;
                txtdaterecieved.Text = DateTime.Now.ToString("MM/dd/yyyy");
            }
        }


        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            cmbmechanic1.Text = "Assign Mechanic";
            cmbmechanic2.Text = "Assign Mechanic";
            cmbstatus.SelectedIndex = 0;
            txtrepairfee.Text = "";
            txtbikerecieving.Text = "Waiting for Arrival";
            txtdatecomplete.Text = "";
            txtdatedelivered.Text = "";
            txtprice.Text = "";
            txtdaterecieved.Text = "";
        }

        private void txtrepairfee_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (e.g., backspace)
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Allow digits (0-9) and one decimal point (.)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Block the character
            }

            // Ensure only one decimal point is allowed
            if (e.KeyChar == '.' && txtrepairfee.Text.Contains("."))
            {
                e.Handled = true; // Block the second decimal point
            }
        }


        private void txtdatecomplete_TextChanged(object sender, EventArgs e)
        {
            // Check if txtmethod2 is "Delivery" and if txtdatecomplete is not empty
            if (txtmethod2.Text == "Delivery" && !string.IsNullOrEmpty(txtdatecomplete.Text))
            {
                // Update txtdatedelivered with the value of txtdatecomplete
                txtdatedelivered.Text = txtdatecomplete.Text;
            }
            else
            {
                txtdatedelivered.Text = "";
            }
        }

        private void txtrepairfee_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtrepairfee.Text))
            {
                errorProvider1.SetError(txtrepairfee, "Insert Price");
                errorCount++;
                txtprice.Text = txtservicefee.Text; // fallback: show only service fee
                return;
            }

            // Try parsing values safely
            bool isServiceFeeValid = int.TryParse(txtservicefee.Text, out servicefee);
            bool isRepairFeeValid = int.TryParse(txtrepairfee.Text, out repairfee);

            if (isServiceFeeValid && isRepairFeeValid)
            {
                totalprice = servicefee + repairfee;
                txtprice.Text = totalprice.ToString("0.00");
                errorProvider1.SetError(txtrepairfee, string.Empty);
                if (errorCount > 0) errorCount--;
            }
            else
            {
                txtprice.Text = string.Empty;
                errorProvider1.SetError(txtrepairfee, "Invalid number");
                errorCount++;
            }
        }


        // A list to store all mechanics for easy reloading.
        private List<string> allMechanics = new List<string>();
        private bool isUpdatingComboBoxes = false; // Flag to prevent recursive event calls

        // Constructor
        

        // Form load event
        private void frmUpdateRepair_Load(object sender, EventArgs e)
        {
            LoadRepairDetails();
            LoadMechanics();
            txtrepairfee.Text = viewrepairfee;
            if (cmbbikestatus.SelectedIndex == 2)
            {
                cmbstatus.Enabled = false;
                txtrepairfee.Enabled = false;
            }
        }



        private void ValidateForm()
        {
            if (cmbstatus.SelectedItem.ToString() == "REPAIRING")
            {
                if (cmbmechanic1.SelectedIndex <= 0)
                {
                    errorProvider1.SetError(cmbmechanic1, "Please assign a mechanic.");
                    errorCount++;
                }
                else
                {
                    errorProvider1.SetError(cmbmechanic1, string.Empty);
                }

                errorProvider1.SetError(cmbmechanic2, string.Empty);
            }
            else
            {
                errorProvider1.SetError(cmbmechanic1, string.Empty);
                errorProvider1.SetError(cmbmechanic2, string.Empty);
            }
        }


        private void cmbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbstatus.SelectedItem != null)
            {
                string selectedStatus = cmbstatus.SelectedItem.ToString();

                errorCount = 0;
                errorProvider1.Clear();

                // Check for "REPAIR SUCCESS" or selected index 3
                if (selectedStatus == "REPAIR SUCCESS" || cmbstatus.SelectedIndex == 3)
                {
                    cmbmechanic1.Enabled = false;
                    cmbmechanic2.Enabled = false;
                    cmbbikestatus.Enabled = true;
                    txtbikerecieving.Text = "Received";
                    txtdatecomplete.Text = DateTime.Now.ToString("MM/dd/yyyy");

                    if (int.TryParse(viewservicefee, out servicefee))
                    {
                        if (txtmethod1.Text == "Drop Off")
                            servicefee += 50;
                        if (txtmethod2.Text == "Delivery")
                            servicefee += 50;

                        txtservicefee.Text = servicefee.ToString("0");
                    }
                    else
                    {
                        txtservicefee.Text = "0"; // fallback in case parsing fails
                    }

                    txtrepairfee.Text = viewrepairfee; // ✅ set repair fee properly
                    txtrepairfee.Enabled = true;

                    if (string.IsNullOrEmpty(txtrepairfee.Text) || !decimal.TryParse(txtrepairfee.Text, out _))
                    {
                        errorProvider1.SetError(txtrepairfee, "Insert price");
                        errorCount++;
                    }
                    else
                    {
                        errorProvider1.SetError(txtrepairfee, string.Empty);
                    }
                }
                else if (selectedStatus == "IN QUEUE")
                {
                    cmbmechanic1.Enabled = true;
                    cmbmechanic2.Enabled = true;
                    cmbmechanic1.Text = viewmechanic1;
                    cmbmechanic2.Text = viewmechanic2;
                    txtbikerecieving.Text = "Received";
                    txtrepairfee.Text = "";
                    txtprice.Text = "";
                    ResetFields();
                }
                else if (selectedStatus == "WAITING FOR ARRIVAL")
                {
                    cmbmechanic1.Enabled = false;
                    cmbmechanic2.Enabled = false;
                    cmbmechanic1.Text = "Assign Mechanic";
                    cmbmechanic2.Text = "Assign Mechanic";
                    txtbikerecieving.Text = "Waiting for Arrival";
                    txtrepairfee.Text = "";
                    txtprice.Text = "";
                    ResetFields();
                }
                else if (selectedStatus == "REPAIRING")
                {
                    cmbmechanic1.Enabled = true;
                    cmbmechanic2.Enabled = true;
                    txtdatecomplete.Text = "";
                    txtrepairfee.Text = "";
                    txtprice.Text = "";
                    ValidateForm();
                }
                else
                {
                    txtdatecomplete.Text = string.Empty;
                    ResetFields();
                    txtrepairfee.Enabled = true;
                }

                // If status is not "REPAIR SUCCESS", disable and clear txtrepairfee
                if (selectedStatus != "REPAIR SUCCESS" && cmbstatus.SelectedIndex != 3)
                {
                    txtservicefee.Text = viewservicefee;
                    txtrepairfee.Text = viewrepairfee;
                    txtrepairfee.Enabled = true;
                    cmbbikestatus.Enabled = false;
                    cmbbikestatus.Text = viewpickupdropoff;
                }

                // Update bike status based on the selected method
                UpdateBikeStatusBasedOnMethod();
            }
        }





        private void UpdateBikeStatusBasedOnMethod()
        {

            cmbbikestatus.Items.Clear();
            cmbbikestatus.Items.Add("Select Status");

            if (txtmethod2.Text == "Delivery" || txtmethod2.Text == "DELIVERY")
            {
                cmbbikestatus.Items.Add("Delivering");
                cmbbikestatus.Items.Add("Received");
            }
            else if (txtmethod2.Text == "Pick Up")
            {
                cmbbikestatus.Items.Add("Waiting for Pick Up");
                cmbbikestatus.Items.Add("Received");
            }

            // Set the default selection to "Select Status"
            cmbbikestatus.SelectedItem = viewpickupdropoff;
            if (cmbbikestatus.Text == "Received")
            {
                cmbstatus.Enabled = false;
                txtrepairfee.Enabled=false;
            }
        }



        // Resets form fields when not in use
        private void ResetFields()
        {
            txtdatecomplete.Text = viewdatecomplete;
            txtservicefee.Text = viewservicefee;
            txtrepairfee.Text = viewrepairfee;
            txtprice.Text = viewprice;
            txtrepairfee.Text = viewrepairfee;
            cmbbikestatus.Enabled = false;
            cmbmechanic1.Text = viewmechanic1;
            cmbmechanic2.Text = viewmechanic2;
        }

        // Reloads mechanics into cmbmechanic1 excluding a given username
        private void ReloadMechanic1List(string exclude)
        {
            if (isUpdatingComboBoxes) return;
            isUpdatingComboBoxes = true;

            string prevSelected = cmbmechanic1.SelectedItem?.ToString();
            cmbmechanic1.Items.Clear();  // Clear existing items before adding new ones
            cmbmechanic1.Items.Add("Assign Mechanic");  // Add the "Assign Mechanic" option

            // Add all mechanics if no exclusion, else add mechanics excluding the selected one
            foreach (string mech in allMechanics)
            {
                if (string.IsNullOrEmpty(exclude) || mech != exclude)
                    cmbmechanic1.Items.Add(mech);
            }

            // Re-select the previously selected mechanic if still in the list, otherwise reset to default
            if (prevSelected != null && cmbmechanic1.Items.Contains(prevSelected))
                cmbmechanic1.SelectedItem = prevSelected;
            else
                cmbmechanic1.SelectedIndex = 0;

            isUpdatingComboBoxes = false;
        }

        private void ReloadMechanic2List(string exclude)
        {
            if (isUpdatingComboBoxes) return;
            isUpdatingComboBoxes = true;

            string prevSelected = cmbmechanic2.SelectedItem?.ToString();
            cmbmechanic2.Items.Clear();  // Clear existing items before adding new ones
            cmbmechanic2.Items.Add("Assign Mechanic");  // Add the "Assign Mechanic" option

            // Add all mechanics if no exclusion, else add mechanics excluding the selected one
            foreach (string mech in allMechanics)
            {
                if (string.IsNullOrEmpty(exclude) || mech != exclude)
                    cmbmechanic2.Items.Add(mech);
            }

            // Re-select the previously selected mechanic if still in the list, otherwise reset to default
            if (prevSelected != null && cmbmechanic2.Items.Contains(prevSelected))
                cmbmechanic2.SelectedItem = prevSelected;
            else
                cmbmechanic2.SelectedIndex = 0;

            isUpdatingComboBoxes = false;
        }

        private void cmbmechanic1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUpdatingComboBoxes)
            {
                // If "Assign Mechanic" is selected, reset the other combo box
                if (cmbmechanic1.SelectedIndex == 0) // "Assign Mechanic" selected
                {
                    ReloadMechanic2List(""); // Allow all mechanics in the second combo box
                }
                else if (cmbmechanic1.SelectedIndex > 0) // If a mechanic is selected
                {
                    string selected = cmbmechanic1.SelectedItem.ToString();
                    ReloadMechanic2List(selected); // Exclude selected mechanic from the second combo box
                }
            }

            // Validation
            ValidateForm();

            // Hide "REPAIR SUCCESS" from status if no mechanic is assigned
            if (cmbmechanic1.SelectedItem != null && cmbmechanic1.SelectedItem.ToString() == "Assign Mechanic")
            {
                if (cmbstatus.Items.Contains("REPAIR SUCCESS"))
                {
                    cmbstatus.Items.Remove("REPAIR SUCCESS");

                    // If it was already selected, reset the selection
                    if (cmbstatus.Text == "REPAIR SUCCESS")
                        cmbstatus.SelectedIndex = 0;
                }
            }
            else
            {
                // Re-add "REPAIR SUCCESS" if it's missing
                if (!cmbstatus.Items.Contains("REPAIR SUCCESS"))
                {
                    cmbstatus.Items.Add("REPAIR SUCCESS");
                }
            }
        }


        // Event handler for cmbmechanic2 selection change
        private void cmbmechanic2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUpdatingComboBoxes)
            {
                // If "Assign Mechanic" is selected, reset the first combo box
                if (cmbmechanic2.SelectedIndex == 0) // "Assign Mechanic" selected
                {
                    ReloadMechanic1List(""); // Allow all mechanics in the first combo box
                }
                else if (cmbmechanic2.SelectedIndex > 0) // If a mechanic is selected
                {
                    string selected = cmbmechanic2.SelectedItem.ToString();
                    ReloadMechanic1List(selected); // Exclude selected mechanic from the first combo box
                }
            }

            ValidateForm();
        }


        // Load mechanics into both combo boxes and store in allMechanics list
        private void LoadMechanics()
        {
            string query = "SELECT username FROM tbluser WHERE usertype = 'MECHANIC' AND status = 'ACTIVE'";
            try
            {
                DataTable dt = updaterepair.GetData(query);
                foreach (DataRow row in dt.Rows)
                {
                    allMechanics.Add(row["username"].ToString());
                }

                // Add mechanics from allMechanics list, check if they're already in the combo boxes to avoid duplication
                foreach (string m in allMechanics)
                {
                    if (!cmbmechanic1.Items.Contains(m))
                        cmbmechanic1.Items.Add(m);
                    if (!cmbmechanic2.Items.Contains(m))
                        cmbmechanic2.Items.Add(m);
                }

                // Add viewmechanic1 and viewmechanic2 if they're not already present
                if (!cmbmechanic1.Items.Contains(viewmechanic1))
                {
                    cmbmechanic1.Items.Add(viewmechanic1);
                }
                if (!cmbmechanic2.Items.Contains(viewmechanic2))
                {
                    cmbmechanic2.Items.Add(viewmechanic2);
                }

                txtprice.Text = viewprice;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading mechanics: " + ex.Message);
            }
        }


        private void btndone_Click(object sender, EventArgs e)
        {
            // Validate if the required fields are filled
            if (string.IsNullOrEmpty(txtrepairfee.Text) || string.IsNullOrEmpty(cmbstatus.Text) || cmbmechanic1.SelectedIndex == 0)
            {
                MessageBox.Show("Please fill in all required fields before proceeding.");
                return;
            }

            try
            {
                // Construct the update query
                string updateQuery = $"UPDATE tblrepair SET price = {txtrepairfee.Text}, bikestatus = '{cmbstatus.Text}', " +
                                     $"mechanic1 = '{cmbmechanic1.SelectedItem}', mechanic2 = '{cmbmechanic2.SelectedItem}', " +
                                     $"daterecieved = '{txtdaterecieved.Text}', datecomplete = '{txtdatecomplete.Text}', repairfee = '{txtrepairfee.Text}', servicefee = '{txtservicefee.Text}', price = '{txtprice.Text}', pickupdropoff = '{cmbbikestatus.SelectedItem}'," +
                                     $"datedelivered = '{txtdatedelivered.Text}' WHERE code = '{viewcode}'";
                

                // Execute the update query using the executeSQL method
                updaterepair.executeSQL(updateQuery);

                // Notify user of success
                MessageBox.Show("Repair details updated successfully.");

                // Optionally, refresh the parent form's data (if needed)
                parentForm.RefreshData(); // Make sure parentForm has a method to refresh the data

                // Close the current form after completion
                this.Close();
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during the update
                MessageBox.Show($"Error updating repair details: {ex.Message}");
            }
        }
        private void LoadRepairDetails()
        {
            string query = $"SELECT * FROM tblrepair WHERE code = '{viewcode}'";
            DataTable dt = updaterepair.GetData(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                // Set the form fields from the data retrieved
                txtuser.Text = row["username"].ToString();
                txtcontact.Text = row["contact"].ToString();
                txtbranch.Text = row["branch"].ToString();
                txtconcern.Text = row["concern"].ToString();
                txtmethod1.Text = row["method1"].ToString();
                txtmethod2.Text = row["method2"].ToString();
                cmbmechanic1.Text = row["mechanic1"].ToString();
                cmbmechanic2.Text = row["mechanic2"].ToString();
                cmbbikestatus.Text = row["pickupdropoff"].ToString();
                txtaddress.Text = row["address"].ToString();
                txtservicefee.Text = row["servicefee"].ToString();
                txtrepairfee.Text = row["repairfee"].ToString();
                txtprice.Text = row["price"].ToString();
                cmbstatus.Text = row["bikestatus"].ToString();

                // Ensure cmbmechanic1 and cmbmechanic2 have items before setting the SelectedItem
                if (cmbmechanic1.Items.Count > 0)
                {
                    cmbmechanic1.SelectedItem = row["mechanic1"].ToString();
                    if (cmbmechanic1.SelectedItem == null)
                    {
                        cmbmechanic1.SelectedIndex = 0; // Default to the first item if no match is found
                    }
                }

                if (cmbmechanic2.Items.Count > 0)
                {
                    cmbmechanic2.SelectedItem = row["mechanic2"].ToString();
                    if (cmbmechanic2.SelectedItem == null)
                    {
                        cmbmechanic2.SelectedIndex = 0; // Default to the first item if no match is found
                    }
                }

                // Parse and set the date fields
                DateTime parsedDate;
                txtdaterequested.Text = DateTime.TryParse(viewdaterequested, out parsedDate) ? parsedDate.ToString("yyyy-MM-dd") : string.Empty;
                txtdatecomplete.Text = DateTime.TryParse(viewdatecomplete, out parsedDate) ? parsedDate.ToString("yyyy-MM-dd") : string.Empty;
                txtdatedelivered.Text = DateTime.TryParse(viewdatedelivered, out parsedDate) ? parsedDate.ToString("yyyy-MM-dd") : string.Empty;
                txtdaterecieved.Text = DateTime.TryParse(viewdaterecieved, out parsedDate) ? parsedDate.ToString("yyyy-MM-dd") : string.Empty;

                // Call cmbstatus_SelectedIndexChanged to update the state of txtrepairfee after loading repair details
                cmbstatus_SelectedIndexChanged(cmbstatus, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Repair record not found.");
                this.Close();
            }
        }

    }
}
