namespace JS_Automation
{
    partial class ImportLockData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportLockData));
            this.txtbox_Filepath = new System.Windows.Forms.TextBox();
            this.btn_browse = new System.Windows.Forms.Button();
            this.btn_Loadfile = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtbox_Filepath
            // 
            this.txtbox_Filepath.Location = new System.Drawing.Point(40, 22);
            this.txtbox_Filepath.Multiline = true;
            this.txtbox_Filepath.Name = "txtbox_Filepath";
            this.txtbox_Filepath.Size = new System.Drawing.Size(242, 25);
            this.txtbox_Filepath.TabIndex = 0;
            // 
            // btn_browse
            // 
            this.btn_browse.Location = new System.Drawing.Point(40, 47);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(242, 33);
            this.btn_browse.TabIndex = 2;
            this.btn_browse.Text = "&Browse File";
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // btn_Loadfile
            // 
            this.btn_Loadfile.Location = new System.Drawing.Point(40, 92);
            this.btn_Loadfile.Name = "btn_Loadfile";
            this.btn_Loadfile.Size = new System.Drawing.Size(135, 31);
            this.btn_Loadfile.TabIndex = 3;
            this.btn_Loadfile.Text = "Load File";
            this.btn_Loadfile.UseVisualStyleBackColor = true;
            this.btn_Loadfile.Click += new System.EventHandler(this.btn_Loadfile_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(181, 92);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(101, 31);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // ImportLockData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 150);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_Loadfile);
            this.Controls.Add(this.btn_browse);
            this.Controls.Add(this.txtbox_Filepath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportLockData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Excel Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbox_Filepath;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.Button btn_Loadfile;
        private System.Windows.Forms.Button btn_cancel;
    }
}