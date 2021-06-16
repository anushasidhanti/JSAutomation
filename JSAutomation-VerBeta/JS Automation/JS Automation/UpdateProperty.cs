using PersistenceLayer;
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

namespace JS_Automation
{
    public partial class UpdateProperty : Form
    {
        private Dictionary<string, PropertyUpdateDB.UpdateStatusFields> dctSelectedRows;
        
        public UpdateProperty(Dictionary<string, PropertyUpdateDB.UpdateStatusFields> dctSelectedRows)
        {
            InitializeComponent();
            this.dgvSelectedPipelines.DataSource = dctSelectedRows.Values.ToList();
            this.dctSelectedRows = dctSelectedRows;
        }

        private void UpdateProperty_Load(object sender, EventArgs e)
        {
            List<objectStatus> oStatuslist = new List<objectStatus>();

            oStatuslist.Add(new objectStatus() { cList = 1, oSts = "Working" });
            oStatuslist.Add(new objectStatus() { cList = 2, oSts = "In Review" });
            oStatuslist.Add(new objectStatus() { cList = 4, oSts = "Rejected" });
            oStatuslist.Add(new objectStatus() { cList = 8, oSts = "Approved" });

            comboBox1.DataSource = oStatuslist;
            comboBox1.DisplayMember = "oSts";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Event Run when select value in Combo Box
        {
            
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            objectStatus oSts1 = comboBox1.SelectedItem as objectStatus;            
            String codlistvalue = Convert.ToString(oSts1.cList);

            if (codlistvalue == "")
            {
                MessageBox.Show("Select Object Status");
            }
            else
            {
                try
                {
                    (new PropertyUpdateDB()).updateProperty(this.dctSelectedRows.Keys, codlistvalue);

                    MessageBox.Show("PipeLine Status Modify");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Close();
                }
            }
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
