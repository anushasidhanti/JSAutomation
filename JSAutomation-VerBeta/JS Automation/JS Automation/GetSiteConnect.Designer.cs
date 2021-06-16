namespace JS_Automation
    {
    partial class GetSiteConnect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetSiteConnect));
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtConnectString = new System.Windows.Forms.TextBox();
            this.txtSchemaString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnect_Site = new System.Windows.Forms.Button();
            this.btnClose_Site = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radOracle = new System.Windows.Forms.RadioButton();
            this.radMSSQL = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkbxCatalog = new System.Windows.Forms.CheckBox();
            this.chkbxDrawReport = new System.Windows.Forms.CheckBox();
            this.chkbxWBS = new System.Windows.Forms.CheckBox();
            this.chkbxSystem = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(175, 13);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(209, 22);
            this.txtServer.TabIndex = 0;
            // 
            // txtConnectString
            // 
            this.txtConnectString.Location = new System.Drawing.Point(175, 43);
            this.txtConnectString.Name = "txtConnectString";
            this.txtConnectString.Size = new System.Drawing.Size(209, 22);
            this.txtConnectString.TabIndex = 1;
            this.txtConnectString.TextChanged += new System.EventHandler(this.txtConnectString_TextChanged_1);
            // 
            // txtSchemaString
            // 
            this.txtSchemaString.Location = new System.Drawing.Point(175, 72);
            this.txtSchemaString.Name = "txtSchemaString";
            this.txtSchemaString.Size = new System.Drawing.Size(209, 22);
            this.txtSchemaString.TabIndex = 2;
            this.txtSchemaString.TextChanged += new System.EventHandler(this.txtSchemaString_TextChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Site connect string:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Schema connect string:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btnConnect_Site
            // 
            this.btnConnect_Site.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConnect_Site.Location = new System.Drawing.Point(396, 121);
            this.btnConnect_Site.Name = "btnConnect_Site";
            this.btnConnect_Site.Size = new System.Drawing.Size(97, 45);
            this.btnConnect_Site.TabIndex = 7;
            this.btnConnect_Site.Text = "Connect";
            this.btnConnect_Site.UseVisualStyleBackColor = true;
            this.btnConnect_Site.Click += new System.EventHandler(this.btnConnect_Site_Click);
            // 
            // btnClose_Site
            // 
            this.btnClose_Site.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Site.Location = new System.Drawing.Point(396, 179);
            this.btnClose_Site.Name = "btnClose_Site";
            this.btnClose_Site.Size = new System.Drawing.Size(97, 42);
            this.btnClose_Site.TabIndex = 8;
            this.btnClose_Site.Text = "Close";
            this.btnClose_Site.UseVisualStyleBackColor = true;
            this.btnClose_Site.Click += new System.EventHandler(this.btnClose_Site_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radOracle);
            this.groupBox1.Controls.Add(this.radMSSQL);
            this.groupBox1.Location = new System.Drawing.Point(396, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 90);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DB Types";
            // 
            // radOracle
            // 
            this.radOracle.AutoSize = true;
            this.radOracle.Location = new System.Drawing.Point(46, 58);
            this.radOracle.Name = "radOracle";
            this.radOracle.Size = new System.Drawing.Size(71, 21);
            this.radOracle.TabIndex = 1;
            this.radOracle.Text = "Oracle";
            this.radOracle.UseVisualStyleBackColor = true;
            // 
            // radMSSQL
            // 
            this.radMSSQL.AutoSize = true;
            this.radMSSQL.Checked = true;
            this.radMSSQL.Location = new System.Drawing.Point(46, 23);
            this.radMSSQL.Name = "radMSSQL";
            this.radMSSQL.Size = new System.Drawing.Size(82, 21);
            this.radMSSQL.TabIndex = 0;
            this.radMSSQL.TabStop = true;
            this.radMSSQL.Text = "MS-SQL";
            this.radMSSQL.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkbxCatalog);
            this.groupBox2.Controls.Add(this.chkbxDrawReport);
            this.groupBox2.Controls.Add(this.chkbxWBS);
            this.groupBox2.Controls.Add(this.chkbxSystem);
            this.groupBox2.Location = new System.Drawing.Point(15, 115);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(342, 110);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "S3D PBS Type";
            // 
            // chkbxCatalog
            // 
            this.chkbxCatalog.AutoSize = true;
            this.chkbxCatalog.Location = new System.Drawing.Point(192, 65);
            this.chkbxCatalog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkbxCatalog.Name = "chkbxCatalog";
            this.chkbxCatalog.Size = new System.Drawing.Size(140, 21);
            this.chkbxCatalog.TabIndex = 3;
            this.chkbxCatalog.Text = "S3D Object Class";
            this.chkbxCatalog.UseVisualStyleBackColor = true;
            // 
            // chkbxDrawReport
            // 
            this.chkbxDrawReport.AutoSize = true;
            this.chkbxDrawReport.Location = new System.Drawing.Point(14, 66);
            this.chkbxDrawReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkbxDrawReport.Name = "chkbxDrawReport";
            this.chkbxDrawReport.Size = new System.Drawing.Size(159, 21);
            this.chkbxDrawReport.TabIndex = 2;
            this.chkbxDrawReport.Text = "Drawing Report PBS";
            this.chkbxDrawReport.UseVisualStyleBackColor = true;
            // 
            // chkbxWBS
            // 
            this.chkbxWBS.AutoSize = true;
            this.chkbxWBS.Location = new System.Drawing.Point(192, 30);
            this.chkbxWBS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkbxWBS.Name = "chkbxWBS";
            this.chkbxWBS.Size = new System.Drawing.Size(92, 21);
            this.chkbxWBS.TabIndex = 1;
            this.chkbxWBS.Text = "WBS PBS";
            this.chkbxWBS.UseVisualStyleBackColor = true;
            // 
            // chkbxSystem
            // 
            this.chkbxSystem.AutoSize = true;
            this.chkbxSystem.Location = new System.Drawing.Point(14, 31);
            this.chkbxSystem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkbxSystem.Name = "chkbxSystem";
            this.chkbxSystem.Size = new System.Drawing.Size(107, 21);
            this.chkbxSystem.TabIndex = 0;
            this.chkbxSystem.Text = "System PBS";
            this.chkbxSystem.UseVisualStyleBackColor = true;
            // 
            // GetSiteConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 243);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose_Site);
            this.Controls.Add(this.btnConnect_Site);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSchemaString);
            this.Controls.Add(this.txtConnectString);
            this.Controls.Add(this.txtServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetSiteConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Get Connect";
            this.Load += new System.EventHandler(this.GetSiteConnect_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtConnectString;
        private System.Windows.Forms.TextBox txtSchemaString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnConnect_Site;
        private System.Windows.Forms.Button btnClose_Site;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radOracle;
        private System.Windows.Forms.RadioButton radMSSQL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkbxCatalog;
        private System.Windows.Forms.CheckBox chkbxDrawReport;
        private System.Windows.Forms.CheckBox chkbxWBS;
        private System.Windows.Forms.CheckBox chkbxSystem;
    }
    }