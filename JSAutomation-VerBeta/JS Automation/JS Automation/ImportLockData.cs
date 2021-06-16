using System;
using System.Windows.Forms;

namespace JS_Automation
{
    public partial class ImportLockData : Form
    {
        public string excelFullPath = "";
        public static int piperun_chk = 0;
        public ImportLockData()
        {
            InitializeComponent();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Excel Files (*.xlsx)|*.xlsx|Excel Files (*.xls)|*.xls|All Files (*.*)|*.*";
            dlgOpen.Title = "Excel File";
            dlgOpen.DefaultExt = "xlsx";

            DialogResult result = dlgOpen.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
            if (result == DialogResult.OK)
            {
                folderPath = dlgOpen.FileName;
            }
            Cursor.Current = Cursors.WaitCursor;
            excelFullPath = folderPath;
            txtbox_Filepath.Text = excelFullPath;
        }

        private void btn_Loadfile_Click(object sender, EventArgs e)
        {
            string strpath = txtbox_Filepath.Text;
            string piperunid = "";
            string piperunDMaxt = "";
            string piperunDMint = "";
            string piperunDMaxp = "";
            string piperunDMinp = "";
            string piperunStatus = "";

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(strpath);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int i = 2; i <= rowCount; i++)
            {
                MessageBox.Show(rowCount.ToString());
                // MessageBox.Show(xlRange.Cells[i, j].Value2.ToString());
                if (xlRange.Cells[i, 1].Value2 == null)
                {
                    break;
                }
                piperunid = xlRange.Cells[i, 3].Value2.ToString();
                piperunDMaxt = xlRange.Cells[i, 4].Value2.ToString();
                piperunDMint = xlRange.Cells[i, 5].Value2.ToString();
                piperunDMaxp = xlRange.Cells[i, 6].Value2.ToString();
                piperunDMinp = xlRange.Cells[i, 7].Value2.ToString();
                piperunStatus = xlRange.Cells[i, 8].Value2.ToString();

                MessageBox.Show(piperunStatus);



            }
            MessageBox.Show("Done");
            //xlWorkbook.Close();
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);             
            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
