using System;
using System.Windows.Forms;

namespace JS_Automation
{
    public partial class frmWaitForm : Form
    {
        public bool shutDownThread = false;
        public frmWaitForm()
        {
            InitializeComponent();            
        }

        public void startTimer()
        {
            this.shutDownThread = false;
            this.timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (this.bunifuCircleProgressbar1.Value == this.bunifuCircleProgressbar1.MaxValue)
                this.bunifuCircleProgressbar1.Value = 30;
            this.bunifuCircleProgressbar1.Value++;

            if (shutDownThread)
            {
                this.Close();
                this.timer1.Stop();
            }
        }
    }
}
