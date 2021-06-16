using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JS_Automation
{
    public class QueryToBeSaved
    {
        public Guid queryID { get; set; }
        public String query { get; set; }
    }
    public partial class QueryEditor : Form
    {
        
        public QueryEditor(String query)
        {
            InitializeComponent();
            this.richTextBox1.Text = query;
        }

        public String getModifiedQuery
        {
            get
            {
                return this.richTextBox1.Text;
            }
        }
    }
}
