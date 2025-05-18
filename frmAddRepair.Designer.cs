namespace CS311A2024_DATABASE
{
    partial class frmAddRepair
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbmethod2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtaddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtcontact = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbmethod1 = new System.Windows.Forms.ComboBox();
            this.btndone = new System.Windows.Forms.Button();
            this.btnrefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.txtconcern = new System.Windows.Forms.TextBox();
            this.cmbbranch = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(380, 246);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "If the repair is finished";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(347, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 31;
            // 
            // cmbmethod2
            // 
            this.cmbmethod2.FormattingEnabled = true;
            this.cmbmethod2.Items.AddRange(new object[] {
            "Pick Up",
            "Delivery"});
            this.cmbmethod2.Location = new System.Drawing.Point(495, 243);
            this.cmbmethod2.Name = "cmbmethod2";
            this.cmbmethod2.Size = new System.Drawing.Size(192, 21);
            this.cmbmethod2.TabIndex = 30;
            this.cmbmethod2.Text = "Select Method";
            this.cmbmethod2.SelectedIndexChanged += new System.EventHandler(this.cmbmethod2_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Concern";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(345, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Place your address for pick up";
            // 
            // txtaddress
            // 
            this.txtaddress.Enabled = false;
            this.txtaddress.Location = new System.Drawing.Point(495, 106);
            this.txtaddress.Multiline = true;
            this.txtaddress.Name = "txtaddress";
            this.txtaddress.Size = new System.Drawing.Size(192, 131);
            this.txtaddress.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(377, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Your Contact Number:";
            // 
            // txtcontact
            // 
            this.txtcontact.Enabled = false;
            this.txtcontact.Location = new System.Drawing.Point(495, 80);
            this.txtcontact.Name = "txtcontact";
            this.txtcontact.Size = new System.Drawing.Size(192, 20);
            this.txtcontact.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Requirements";
            // 
            // cmbmethod1
            // 
            this.cmbmethod1.FormattingEnabled = true;
            this.cmbmethod1.Items.AddRange(new object[] {
            "Pick Up",
            "Drop Off"});
            this.cmbmethod1.Location = new System.Drawing.Point(495, 53);
            this.cmbmethod1.Name = "cmbmethod1";
            this.cmbmethod1.Size = new System.Drawing.Size(192, 21);
            this.cmbmethod1.TabIndex = 23;
            this.cmbmethod1.Text = "Select Method";
            this.cmbmethod1.SelectedIndexChanged += new System.EventHandler(this.cmbmethod1_SelectedIndexChanged);
            // 
            // btndone
            // 
            this.btndone.Location = new System.Drawing.Point(566, 316);
            this.btndone.Name = "btndone";
            this.btndone.Size = new System.Drawing.Size(75, 23);
            this.btndone.TabIndex = 22;
            this.btndone.Text = "Done";
            this.btndone.UseVisualStyleBackColor = true;
            this.btndone.Click += new System.EventHandler(this.btndone_Click);
            // 
            // btnrefresh
            // 
            this.btnrefresh.Location = new System.Drawing.Point(485, 316);
            this.btnrefresh.Name = "btnrefresh";
            this.btnrefresh.Size = new System.Drawing.Size(75, 23);
            this.btnrefresh.TabIndex = 21;
            this.btnrefresh.Text = "Refresh";
            this.btnrefresh.UseVisualStyleBackColor = true;
            this.btnrefresh.Click += new System.EventHandler(this.btnrefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(356, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Please fill in all of the";
            // 
            // btncancel
            // 
            this.btncancel.Location = new System.Drawing.Point(337, 316);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 19;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            // 
            // txtconcern
            // 
            this.txtconcern.Location = new System.Drawing.Point(73, 68);
            this.txtconcern.Multiline = true;
            this.txtconcern.Name = "txtconcern";
            this.txtconcern.Size = new System.Drawing.Size(258, 358);
            this.txtconcern.TabIndex = 18;
            // 
            // cmbbranch
            // 
            this.cmbbranch.FormattingEnabled = true;
            this.cmbbranch.Items.AddRange(new object[] {
            "Arellano Extension Street",
            "Estrada Street"});
            this.cmbbranch.Location = new System.Drawing.Point(73, 41);
            this.cmbbranch.Name = "cmbbranch";
            this.cmbbranch.Size = new System.Drawing.Size(249, 21);
            this.cmbbranch.TabIndex = 17;
            this.cmbbranch.Text = "Select Branch";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmAddRepair
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 557);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbmethod2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtaddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtcontact);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbmethod1);
            this.Controls.Add(this.btndone);
            this.Controls.Add(this.btnrefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.txtconcern);
            this.Controls.Add(this.cmbbranch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmAddRepair";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAddRepair";
            this.Load += new System.EventHandler(this.frmAddRepair_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbmethod2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtaddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtcontact;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbmethod1;
        private System.Windows.Forms.Button btndone;
        private System.Windows.Forms.Button btnrefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.TextBox txtconcern;
        private System.Windows.Forms.ComboBox cmbbranch;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}