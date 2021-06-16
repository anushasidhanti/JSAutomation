using PersistenceLayer;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JS_Automation
{
    public partial class ReportSelector : Form
    {
        
        public ReportSelector()
        {
            InitializeComponent();
        }

        public ComboBox getComboBox
        {
            get
            {
                return this.comboBox1;
            }
        }

        private void ReportSelector_Load(object sender, EventArgs e)
        {
            ReportSelectorDB reportSelector = new ReportSelectorDB();
            
            List<ReportSelectorDB.ReportSelectorData> lstReportSelectorData =
                reportSelector.readQueryStore();

            this.comboBox1.DataSource = lstReportSelectorData;
            this.comboBox1.DisplayMember = "QueryName";
            this.comboBox1.ValueMember = "Query";
            
            //var dictQueries = JsonConvert.DeserializeObject<Dictionary<string, object>>(jArray[0].ToString());

            //foreach (KeyValuePair<String, Object> keyValuePair in dictQueries)
            //{

            //    ReportSelectorData reportSelectorData = new ReportSelectorData();
            //    //reportSelectorData.DBName 


            //    object name = dictQueries[keyValuePair.Key];
            //}
        }
    }
}
