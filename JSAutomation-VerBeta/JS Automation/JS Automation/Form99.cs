using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Xml;
using PersistenceLayer;

namespace ConnectionWizard
{
    public partial class Form99 : Form
    {
        string cs = "";
        //used to store the sql connection string 
        string osb = "";
        //used to store the oledb connection string 
        string dbname = "";
        //used o store the name of the database 
        bool noPassword = false;
        bool goodCon = false;

        Form99DB form99DBInstance;

        public Form99()
        {
            form99DBInstance = new Form99DB();
            InitializeComponent();
        }
        /// <summary>
        /// Returns a list of SQL servers
        /// </summary>
        /// <returns></returns>
        private string[] GetSQLServerList()
        {
            return form99DBInstance.GetSQLServerList();
        }

        /// <summary>
        /// Returns a list of user databases on the specified server instance 
        /// </summary>
        /// <param name="serverInstanceName">Name of server and if applicable the instance name: DEV10\SQLEXPRESS or MYSQLSERVER</param>
        /// <param name="useWindowsAuthentication">true if SSPI should be used; otherwise the username and password must be specified.</param>
        /// <param name="username">username of an SQL server user account</param>
        /// <param name="password">password for the SQL account specified by the username above fixed string array containing the list of user databases </param>
        /// <returns></returns>
        private string[] GetSQLDatabaseList(string serverInstanceName, bool useWindowsAuthentication, string username, string password)
        {
            try
            {
                return this.form99DBInstance.GetSQLDatabaseList(serverInstanceName, useWindowsAuthentication, username, password);
            }
            catch(Exception exception)
            {
                MessageBox.Show("Cannot obtain database list:\n" + exception.Message, "Migration Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
        }

        private void BuildConnection()
        {
            if (String.IsNullOrEmpty(cmbServers.Text) || String.IsNullOrEmpty(cmbDatabases.Text))
                return;

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

            if (chkUseWindowsSecurity.Checked)
            {
                csb.DataSource = cmbServers.Text;
                csb.IntegratedSecurity = chkUseWindowsSecurity.Checked;
                csb.InitialCatalog = cmbDatabases.Text;
                cs = csb.ToString();


                osb = "Provider = SQLOLEDB.1;Integrated Security = SSPI;Persist Security Info = False; Initial Catalog = " + cmbDatabases.Text + ";Data Source = " + cmbServers.Text;
            }
            else
            {

                csb.DataSource = cmbServers.Text;
                csb.IntegratedSecurity = chkUseWindowsSecurity.Checked;
                csb.InitialCatalog = cmbDatabases.Text;
                csb.UserID = txtUserName.Text;
                csb.Password = txtPassword.Text;
                cs = csb.ToString();

                osb = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID= " + txtUserName.Text + ";Initial Catalog= " + cmbDatabases.Text + ";Data Source = " + cmbServers.Text;

                dbname = cmbDatabases.Text;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //we'll get a list of servers before the wizard loads so as to avoid the end user 
            //thinking that nothing is happening 
            try
            {
                cmbServers.Items.AddRange(GetSQLServerList());
            }
            catch (Exception ex)
            {
                string mymsg = "There was a problem retrieving information about SQL Servers on your computer or network you may need to enter this information into the wizard manually";
                MessageBox.Show(mymsg, "Error retrieving SQL Server Instances", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //we want to diable the browse for servers button if we have a list of servers  
            if (cmbServers.Items.Count != 0)
            {
                btnFindServers.Enabled = false;
            }
            else
            {
                btnFindServers.Enabled = true;
            }
        }

        /*Select SQL Server*/

        private void cmbServers_SelectedValueChanged(object sender, EventArgs e)
        {
            if (chkUseWindowsSecurity.Checked)
            {
                cmbDatabases.Items.AddRange(GetSQLDatabaseList(cmbServers.Text, true, txtUserName.Text, txtPassword.Text));
                //now disable the find databses button if we have a list 
                if (cmbDatabases.Items.Count != 0)
                {
                    btnFindDatabases.Enabled = false;
                }
                else
                {
                    btnFindDatabases.Enabled = true;
                }
            }
            else
            {

            } 
        }

        private void btnFindServers_Click(object sender, EventArgs e)
        {
            //it's possible to duplicate the list so we need to ensure that there isn't one already
            if (cmbServers.Items.Count != 0)
            {
            }
            else
            {
                cmbServers.Items.AddRange(GetSQLServerList());
            }
        }

        /*Windows Security*/

        private void chkUseWindowsSecurity_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseWindowsSecurity.Checked)
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                chkBlankPassAllowed.Enabled = false;
            }
            else
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                chkBlankPassAllowed.Enabled = true;
            }
        }

        private void chkUseWindowsSecurity_CheckStateChanged(object sender, EventArgs e)
        {
            //by default the button is checked if this is fire then the user intends using user authentification 
            //firstly reinstate the enabled state of the browse for databases button if its disabled 
            if (btnFindDatabases.Enabled == false)
            {
                btnFindDatabases.Enabled = true;
            }

        }

        /*Select Database*/

        private void cmbDatabases_Click(object sender, EventArgs e)
        {
            if (chkUseWindowsSecurity.Checked == false)
            {
                if (chkBlankPassAllowed.Checked == false)
                {


                    if (string.IsNullOrEmpty(txtUserName.Text))
                    {
                        string msg = "You have unchecked the 'Use Windows Security' checkbox.\n" + "Consequently you must enter a user name";
                        MessageBox.Show(msg, "No User Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUserName.Focus();

                        return;
                    }
                    else if (string.IsNullOrEmpty(txtPassword.Text))
                    {
                        string msg = "A password is required.\n" + "If you know the password is blank, check the 'Blank Password Allowed' box.";

                        MessageBox.Show(msg, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //don't worry no password was deliberate. 
                        //it's possible to get caught in a loop here so add a check to say cancel has been pressed 


                        txtPassword.Focus();
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserName.Text))
                    {
                        string msg = "You have unchecked the 'Use Windows Security' checkbox.\n" + "Consequently you must enter a user name";
                        MessageBox.Show(msg, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUserName.Focus();
                        return;
                    }
                }

                //we have info so process it 
                try
                {


                    cmbDatabases.Items.AddRange(GetSQLDatabaseList(cmbServers.Text, false, txtUserName.Text, txtPassword.Text));
                }
                catch (Exception ex)
                {

                    MessageBox.Show("There has been an error trying to connect to the database with the parameters you entered.\n" + ex.Message, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {

            } 
        }

        private void cmbDatabases_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnFindDatabases_Click(object sender, EventArgs e)
        {
            //it's possible to duplicate the list so we need to ensure that there isn't one already 
            if (cmbDatabases.Items.Count != 0)
            {
            }
            else
            {


                if (chkUseWindowsSecurity.Checked)
                {

                    //we just need to do a check with no details 

                    cmbDatabases.Items.AddRange(GetSQLDatabaseList(cmbServers.Text, true, txtUserName.Text, txtPassword.Text));
                }
                else
                {
                    //we need to do a check with details 


                    cmbDatabases.Items.AddRange(GetSQLDatabaseList(cmbServers.Text, false, txtUserName.Text, txtPassword.Text));
                }
            }
        }

        /*Form Buttons*/

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            BuildConnection();

            try
            {
                this.goodCon = this.form99DBInstance.testConnection(cs);
                if (this.goodCon)
                {
                    MessageBox.Show("Connection to database was successful", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }            
            catch(Exception exception)
            {
                MessageBox.Show("Could not connect to the database \n" + exception.Message, "Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cs))
            {
                //we have a connection string 
                if (this.goodCon == true)
                {
                    //it was validated so we can go ahead and save it 
                    //firstly as xml 
                    string fname = Path.GetDirectoryName(Application.LocalUserAppDataPath) + "\\ConnectionStrings.xml";

                    XmlTextWriter writer = new XmlTextWriter(fname, null);
                    writer.Formatting = Formatting.Indented;
                    WriteConnectionStrings(writer, cs, osb);
                    writer.Close();

                    this.Close();
                    //In order to use the new connection string you have just created the application needsto close.
                    //When you restart the application you should load the connection string from the xml file.
                    //Application.Exit();
                }
                else
                {
                    //connection wasn't validated so just close 
                    this.Close();
                }
            } 
        }
        public void WriteConnectionStrings(XmlWriter writer, string sqlcon, string oledbcon)
        {
            writer.WriteStartElement("ConnectionStrings"); 
            writer.WriteElementString("SqlConnectionString", sqlcon);
            writer.WriteElementString("DatabaseName", dbname);
            writer.WriteEndElement();
        } 
    }
}
