namespace CS311A2024_DATABASE
{
    partial class frmCartBuilt
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
            this.btnremovetocart = new System.Windows.Forms.Button();
            this.txtprice = new System.Windows.Forms.TextBox();
            this.txtspecs = new System.Windows.Forms.TextBox();
            this.txtname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnpurchase = new System.Windows.Forms.Button();
            this.btnclose = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtbranch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtmethod2 = new System.Windows.Forms.TextBox();
            this.lblmethod2 = new System.Windows.Forms.Label();
            this.btncancelpurchase = new System.Windows.Forms.Button();
            this.txtstatus = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnremovetocart
            // 
            this.btnremovetocart.Location = new System.Drawing.Point(377, 295);
            this.btnremovetocart.Name = "btnremovetocart";
            this.btnremovetocart.Size = new System.Drawing.Size(148, 23);
            this.btnremovetocart.TabIndex = 27;
            this.btnremovetocart.Text = "Remove to Cart";
            this.btnremovetocart.UseVisualStyleBackColor = true;
            this.btnremovetocart.Click += new System.EventHandler(this.btnremove_Click);
            // 
            // txtprice
            // 
            this.txtprice.Enabled = false;
            this.txtprice.Location = new System.Drawing.Point(110, 137);
            this.txtprice.Name = "txtprice";
            this.txtprice.Size = new System.Drawing.Size(176, 20);
            this.txtprice.TabIndex = 26;
            // 
            // txtspecs
            // 
            this.txtspecs.Enabled = false;
            this.txtspecs.Location = new System.Drawing.Point(377, 72);
            this.txtspecs.Multiline = true;
            this.txtspecs.Name = "txtspecs";
            this.txtspecs.Size = new System.Drawing.Size(369, 174);
            this.txtspecs.TabIndex = 25;
            // 
            // txtname
            // 
            this.txtname.Enabled = false;
            this.txtname.Location = new System.Drawing.Point(90, 40);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(176, 20);
            this.txtname.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(374, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Bike Specifications";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Price:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Bike Name:";
            // 
            // btnpurchase
            // 
            this.btnpurchase.Location = new System.Drawing.Point(598, 295);
            this.btnpurchase.Name = "btnpurchase";
            this.btnpurchase.Size = new System.Drawing.Size(148, 23);
            this.btnpurchase.TabIndex = 32;
            this.btnpurchase.Text = "Purchase";
            this.btnpurchase.UseVisualStyleBackColor = true;
            this.btnpurchase.Click += new System.EventHandler(this.btnpurchase_Click);
            // 
            // btnclose
            // 
            this.btnclose.Location = new System.Drawing.Point(25, 295);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(97, 23);
            this.btnclose.TabIndex = 33;
            this.btnclose.Text = "Close";
            this.btnclose.UseVisualStyleBackColor = true;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(137, 250);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 20);
            this.textBox1.TabIndex = 29;
            this.textBox1.Text = "Added to Cart";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtbranch
            // 
            this.txtbranch.Enabled = false;
            this.txtbranch.Location = new System.Drawing.Point(110, 163);
            this.txtbranch.Name = "txtbranch";
            this.txtbranch.Size = new System.Drawing.Size(176, 20);
            this.txtbranch.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Branch:";
            // 
            // txtmethod2
            // 
            this.txtmethod2.Enabled = false;
            this.txtmethod2.Location = new System.Drawing.Point(110, 218);
            this.txtmethod2.Name = "txtmethod2";
            this.txtmethod2.Size = new System.Drawing.Size(176, 20);
            this.txtmethod2.TabIndex = 37;
            // 
            // lblmethod2
            // 
            this.lblmethod2.AutoSize = true;
            this.lblmethod2.Location = new System.Drawing.Point(58, 221);
            this.lblmethod2.Name = "lblmethod2";
            this.lblmethod2.Size = new System.Drawing.Size(46, 13);
            this.lblmethod2.TabIndex = 36;
            this.lblmethod2.Text = "Method:";
            // 
            // btncancelpurchase
            // 
            this.btncancelpurchase.Location = new System.Drawing.Point(166, 295);
            this.btncancelpurchase.Name = "btncancelpurchase";
            this.btncancelpurchase.Size = new System.Drawing.Size(148, 23);
            this.btncancelpurchase.TabIndex = 38;
            this.btncancelpurchase.Text = "Cancel Purchase";
            this.btncancelpurchase.UseVisualStyleBackColor = true;
            // 
            // txtstatus
            // 
            this.txtstatus.Enabled = false;
            this.txtstatus.Location = new System.Drawing.Point(110, 192);
            this.txtstatus.Name = "txtstatus";
            this.txtstatus.Size = new System.Drawing.Size(176, 20);
            this.txtstatus.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Status:";
            // 
            // frmCartBuilt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 385);
            this.Controls.Add(this.txtstatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btncancelpurchase);
            this.Controls.Add(this.txtmethod2);
            this.Controls.Add(this.lblmethod2);
            this.Controls.Add(this.txtbranch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnclose);
            this.Controls.Add(this.btnpurchase);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnremovetocart);
            this.Controls.Add(this.txtprice);
            this.Controls.Add(this.txtspecs);
            this.Controls.Add(this.txtname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmCartBuilt";
            this.Text = "Cart Built";
            this.Load += new System.EventHandler(this.frmCartBuilt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnremovetocart;
        private System.Windows.Forms.TextBox txtprice;
        private System.Windows.Forms.TextBox txtspecs;
        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnpurchase;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtbranch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtmethod2;
        private System.Windows.Forms.Label lblmethod2;
        private System.Windows.Forms.Button btncancelpurchase;
        private System.Windows.Forms.TextBox txtstatus;
        private System.Windows.Forms.Label label6;
    }
}