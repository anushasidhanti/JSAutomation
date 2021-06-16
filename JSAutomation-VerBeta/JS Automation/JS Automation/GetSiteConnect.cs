using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ingr.SP3D.Common.Middle.Services;
using System.Data.SqlClient;

namespace JS_Automation
{
    public enum TreeviewTypes
    {
        System, WBS, DrawReport, Catalog, Cata
    }
    public partial class GetSiteConnect : Form
    {
        private List<TreeviewTypes> selectedTreeViews;
        private string server;
        private string database;

        public String Server
        {
            get { return server; }
            set { this.server = value; }
        }

        public String Database
        {
            get { return database; }
            set { this.database = value; }
        }

        public List<TreeviewTypes> SelectedTreeViews
        {
            get
            {
                return selectedTreeViews;
            }
        }

        //public string Server
        //{
        //    get
        //    {
        //        return txtServer.Text;
        //    }
        //}
        public string SiteConnectString
        {
            get
            {
                    return txtConnectString.Text;
            }
        }
        public string SchemaConnectString
        {
            get
            {
                    return txtSchemaString.Text;
            }
        }
        public SiteManager.eDBProviderTypes DBType
        {
            get
            {
                if (radMSSQL.Checked)
                {
                    return SiteManager.eDBProviderTypes.MSSQL;
                }
                else
                {
                    return SiteManager.eDBProviderTypes.Oracle;
                }
            }
        }

        public GetSiteConnect()
        {
            InitializeComponent();
            this.selectedTreeViews = new List<TreeviewTypes>();
        }

        private void EnableConnect()
        {
            if ((txtServer.Text.Length > 0) && (txtConnectString.Text.Length > 0) && (txtSchemaString.Text.Length) > 0)
            {
                btnConnect_Site.Enabled = true;
            }
            else
            {
                    btnConnect_Site.Enabled = false;
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
                EnableConnect();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
                Hide();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
                Hide();
        }

        private void txtConnectString_TextChanged(object sender, EventArgs e)
        {
                EnableConnect();
        }

        private void txtSchemaString_TextChanged(object sender, EventArgs e)
        {
                EnableConnect();
        }

        private void GetSiteConnect_Load(object sender, EventArgs e)
        {
            txtServer.Text = this.server;
            txtConnectString.Text = "SSPI";
            txtSchemaString.Text = this.database;
        }

        private void btnConnect_Site_Click(object sender, EventArgs e)
        {
            String Server = (txtServer.Text);
            String Security = (txtConnectString.Text);
            String Catalog = (txtSchemaString.Text);

            if (this.chkbxSystem.Checked)
                this.selectedTreeViews.Add(TreeviewTypes.System);
            if (this.chkbxWBS.Checked)
                this.selectedTreeViews.Add(TreeviewTypes.WBS);
            if (this.chkbxDrawReport.Checked)
                this.selectedTreeViews.Add(TreeviewTypes.DrawReport);
            if (this.chkbxCatalog.Checked)
                this.selectedTreeViews.Add(TreeviewTypes.Catalog);
            if (this.chkbxCatalog.Checked)
                this.selectedTreeViews.Add(TreeviewTypes.WBS);

            this.Close();
        }

        private void btnClose_Site_Click(object sender, EventArgs e)
        {
            DialogResult iExit;
            iExit = MessageBox.Show("Confirm If You Are want to Exit", "Exit",MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (iExit == DialogResult.Yes)
            {
                Application.Exit();
                //this.Close();
            }
                
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtConnectString_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtSchemaString_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
