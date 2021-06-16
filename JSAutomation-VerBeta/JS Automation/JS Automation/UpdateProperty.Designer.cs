namespace JS_Automation
{
    partial class UpdateProperty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateProperty));
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btn_Update = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.dgvSelectedPipelines = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedPipelines)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Modify Status Property";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(57, 202);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(148, 24);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btn_Update
            // 
            this.btn_Update.Location = new System.Drawing.Point(244, 202);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(101, 31);
            this.btn_Update.TabIndex = 3;
            this.btn_Update.Text = "Update";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(380, 202);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(101, 31);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // dgvSelectedPipelines
            // 
            this.dgvSelectedPipelines.AllowUserToAddRows = false;
            this.dgvSelectedPipelines.AllowUserToDeleteRows = false;
            this.dgvSelectedPipelines.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvSelectedPipelines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectedPipelines.Location = new System.Drawing.Point(12, 11);
            this.dgvSelectedPipelines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvSelectedPipelines.Name = "dgvSelectedPipelines";
            this.dgvSelectedPipelines.ReadOnly = true;
            this.dgvSelectedPipelines.RowHeadersWidth = 62;
            this.dgvSelectedPipelines.RowTemplate.Height = 28;
            this.dgvSelectedPipelines.Size = new System.Drawing.Size(553, 120);
            this.dgvSelectedPipelines.TabIndex = 5;
            // 
            // UpdateProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 267);
            this.Controls.Add(this.dgvSelectedPipelines);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateProperty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Property";
            this.Load += new System.EventHandler(this.UpdateProperty_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedPipelines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.DataGridView dgvSelectedPipelines;
    }
}