using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public class Form99DB
    {
        public string[] GetSQLServerList()
        {
            SqlDataSourceEnumerator dse = SqlDataSourceEnumerator.Instance;
            DataTable dt = dse.GetDataSources();

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            string[] SQLServers = new string[dt.Rows.Count];
            int f = -1;
            foreach (DataRow r in dt.Rows)
            {
                string SQLServer = r["ServerName"].ToString();
                string Instance = r["InstanceName"].ToString();
                if (Instance != null && !string.IsNullOrEmpty(Instance))
                {
                    SQLServer += "\\" + Instance;
                }
                SQLServers[System.Math.Max(System.Threading.Interlocked.Increment(ref f), f - 1)] = SQLServer;
            }
            Array.Sort(SQLServers);
            return SQLServers;
        }

        public string[] GetSQLDatabaseList(string serverInstanceName, bool useWindowsAuthentication, string username, string password)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = serverInstanceName;
            //cmbServers.Text 'cboSrcDB.Text 
            csb.IntegratedSecurity = useWindowsAuthentication;
            //csb.TrustServerCertificate = useWindowsAuthentication 
            csb.InitialCatalog = "master";
            if (!useWindowsAuthentication)
            {
                csb.UserID = username;
                csb.Password = password;
            }


            SqlConnection conn = new SqlConnection(csb.ToString());


            // The 'where name like 'f%_' will filter out just those databases begining with "F" or "f" 
            SqlDataAdapter da = new SqlDataAdapter("Select name from sysdatabases ", conn);
            //removed the following from above query so that all databases would be shown 
            // where name like 'f%_' 
            try
            {
                DataTable dt = new DataTable("Databases");
                int rowsAffected = da.Fill(dt);
                if (dt == null || rowsAffected <= 0)
                {
                    return null;
                }

                int f = -1;
                string[] databases = new string[dt.Rows.Count];
                foreach (DataRow r in dt.Rows)
                {
                    databases[System.Math.Max(System.Threading.Interlocked.Increment(ref f), f - 1)] = r["name"].ToString();
                }
                da.Dispose();
                Array.Sort(databases);
                return databases;
            }
            catch (SqlException ex)
            {
                throw (new Exception("Cannot obtain database list:\n" + ex.Message));
            }
            finally
            {
                conn.Close();
            }
        }

        public bool testConnection(string connectionStirng)
        {
            SqlConnection con = new SqlConnection(connectionStirng);
            bool connectionSuccess = false;
            try
            {
                con.Open();                
                connectionSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return connectionSuccess;
        }

    }
}
