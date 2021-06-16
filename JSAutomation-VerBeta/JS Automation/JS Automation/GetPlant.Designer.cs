namespace JS_Automation
    {
    partial class GetPlant
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbox1 = new System.Windows.Forms.TextBox();
            this.txtbox2 = new System.Windows.Forms.TextBox();
            this.txtbox3 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radLocateSite = new System.Windows.Forms.RadioButton();
            this.radOracle = new System.Windows.Forms.RadioButton();
            this.radMSSQL = new System.Windows.Forms.RadioButton();
            this.radUseLastSite = new System.Windows.Forms.RadioButton();
            this.cmdConnectSite = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.lstPlants = new System.Windows.Forms.ListBox();
            this.cmdConnectPlant = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Site connect string:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Schema connect string:";
            // 
            // txtbox1
            // 
            this.txtbox1.Location = new System.Drawing.Point(207, 30);
            this.txtbox1.Name = "txtbox1";
            this.txtbox1.Size = new System.Drawing.Size(222, 22);
            this.txtbox1.TabIndex = 3;
            // 
            // txtbox2
            // 
            this.txtbox2.Location = new System.Drawing.Point(207, 64);
            this.txtbox2.Name = "txtbox2";
            this.txtbox2.Size = new System.Drawing.Size(222, 22);
            this.txtbox2.TabIndex = 4;
            // 
            // txtbox3
            // 
            this.txtbox3.Location = new System.Drawing.Point(207, 96);
            this.txtbox3.Name = "txtbox3";
            this.txtbox3.Size = new System.Drawing.Size(222, 22);
            this.txtbox3.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radLocateSite);
            this.groupBox1.Controls.Add(this.radOracle);
            this.groupBox1.Controls.Add(this.radMSSQL);
            this.groupBox1.Controls.Add(this.radUseLastSite);
            this.groupBox1.Location = new System.Drawing.Point(30, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 111);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Types";
            // 
            // radLocateSite
            // 
            this.radLocateSite.AutoSize = true;
            this.radLocateSite.Location = new System.Drawing.Point(6, 68);
            this.radLocateSite.Name = "radLocateSite";
            this.radLocateSite.Size = new System.Drawing.Size(100, 21);
            this.radLocateSite.TabIndex = 3;
            this.radLocateSite.Text = "Locate Site";
            this.radLocateSite.UseVisualStyleBackColor = true;
            // 
            // radOracle
            // 
            this.radOracle.AutoSize = true;
            this.radOracle.Location = new System.Drawing.Point(150, 68);
            this.radOracle.Name = "radOracle";
            this.radOracle.Size = new System.Drawing.Size(94, 21);
            this.radOracle.TabIndex = 2;
            this.radOracle.Text = "Oracle DB";
            this.radOracle.UseVisualStyleBackColor = true;
            // 
            // radMSSQL
            // 
            this.radMSSQL.AutoSize = true;
            this.radMSSQL.Location = new System.Drawing.Point(150, 39);
            this.radMSSQL.Name = "radMSSQL";
            this.radMSSQL.Size = new System.Drawing.Size(105, 21);
            this.radMSSQL.TabIndex = 1;
            this.radMSSQL.Text = "MS-SQL DB";
            this.radMSSQL.UseVisualStyleBackColor = true;
            // 
            // radUseLastSite
            // 
            this.radUseLastSite.AutoSize = true;
            this.radUseLastSite.Checked = true;
            this.radUseLastSite.Location = new System.Drawing.Point(7, 39);
            this.radUseLastSite.Name = "radUseLastSite";
            this.radUseLastSite.Size = new System.Drawing.Size(108, 21);
            this.radUseLastSite.TabIndex = 0;
            this.radUseLastSite.TabStop = true;
            this.radUseLastSite.Text = "Use last Site";
            this.radUseLastSite.UseVisualStyleBackColor = true;
            // 
            // cmdConnectSite
            // 
            this.cmdConnectSite.Location = new System.Drawing.Point(336, 171);
            this.cmdConnectSite.Name = "cmdConnectSite";
            this.cmdConnectSite.Size = new System.Drawing.Size(119, 35);
            this.cmdConnectSite.TabIndex = 7;
            this.cmdConnectSite.Text = "Connect";
            this.cmdConnectSite.UseVisualStyleBackColor = true;
            this.cmdConnectSite.Click += new System.EventHandler(this.cmdConnectSite_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(336, 254);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(119, 28);
            this.cmdExit.TabIndex = 8;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // lstPlants
            // 
            this.lstPlants.FormattingEnabled = true;
            this.lstPlants.ItemHeight = 16;
            this.lstPlants.Location = new System.Drawing.Point(491, 12);
            this.lstPlants.Name = "lstPlants";
            this.lstPlants.Size = new System.Drawing.Size(177, 260);
            this.lstPlants.TabIndex = 9;
            // 
            // cmdConnectPlant
            // 
            this.cmdConnectPlant.Location = new System.Drawing.Point(336, 212);
            this.cmdConnectPlant.Name = "cmdConnectPlant";
            this.cmdConnectPlant.Size = new System.Drawing.Size(119, 36);
            this.cmdConnectPlant.TabIndex = 10;
            this.cmdConnectPlant.Text = "Connect Plant";
            this.cmdConnectPlant.UseVisualStyleBackColor = true;
            this.cmdConnectPlant.Click += new System.EventHandler(this.cmdConnectPlant_Click);
            // 
            // GetPlant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 322);
            this.Controls.Add(this.cmdConnectPlant);
            this.Controls.Add(this.lstPlants);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdConnectSite);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtbox3);
            this.Controls.Add(this.txtbox2);
            this.Controls.Add(this.txtbox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "GetPlant";
            this.Text = "Get Plant";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbox1;
        private System.Windows.Forms.TextBox txtbox2;
        private System.Windows.Forms.TextBox txtbox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radOracle;
        private System.Windows.Forms.RadioButton radMSSQL;
        private System.Windows.Forms.RadioButton radUseLastSite;
        private System.Windows.Forms.Button cmdConnectSite;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.RadioButton radLocateSite;
        private System.Windows.Forms.ListBox lstPlants;
        private System.Windows.Forms.Button cmdConnectPlant;
        }
    }