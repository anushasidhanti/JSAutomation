using System;
using System.Windows.Forms;
using Ingr.SP3D.Common.Middle.Services;

namespace JS_Automation
{
    public partial class GetPlant : Form
    {
        Site m_oSite = null;

        public GetPlant()
        {
            InitializeComponent();
        }

        private void cmdConnectSite_Click(object sender, EventArgs e)
        {
            try
            {
                if (radUseLastSite.Checked)
                {
                    // connect to the Site database found in the registry
                    m_oSite = MiddleServiceProvider.SiteMgr.ConnectSite();
                }
                else
                {
                    // let user supply site info
                    using (GetSiteConnect frmGetSiteConnect = new GetSiteConnect())
                    {
                        frmGetSiteConnect.ShowDialog(this);
                        if (frmGetSiteConnect.DialogResult == DialogResult.Cancel)
                        {
                            return; // just go away
                        }
                        m_oSite = MiddleServiceProvider.SiteMgr.ConnectSite(frmGetSiteConnect.Server,
                                                                  frmGetSiteConnect.SiteConnectString,
                                                                  frmGetSiteConnect.DBType,
                                                                  frmGetSiteConnect.SchemaConnectString);
                    }

                }
            }
            catch
            {
                //do nothing - error announced below
            }
            if (m_oSite != null)
            {
                if (m_oSite.Plants.Count > 0) // any plants in Site
                {
                    foreach (Plant oPlant in m_oSite.Plants)
                    {
                        lstPlants.Items.Add(oPlant);
                    }
                    cmdConnectSite.Enabled = false; // disallow any more 
                }
            }
            else
            {
                //stsStrip.Text = "Site not found";
            }
        }
        private void lstPlants_SelectedIndexChanged(object sender, EventArgs e)

        {
            if (lstPlants.SelectedIndex != -1) // something selected?
            {
                cmdConnectPlant.Enabled = true;
            }
            else
            {
                cmdConnectPlant.Enabled = false;
            }
        }

        private void cmdConnectPlant_Click(object sender, EventArgs e)
        {
            if (lstPlants.SelectedItem != null)
            {
                // connect to the selected plant
                MiddleServiceProvider.SiteMgr.ActiveSite.OpenPlant((Plant)lstPlants.SelectedItem);
            }

        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            // call transaction abort, no changes
            //MiddleServiceProvider.TransactionMgr.Abort();
            //MiddleServiceProvider.Cleanup();
            //base.Dispose(true);
            //Application.Exit();
            Hide();
        }
    }
}
