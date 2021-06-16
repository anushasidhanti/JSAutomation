using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PersistenceLayer
{
    public class MainFormDBAccess
    {
        private string connectionString;
       

        public MainFormDBAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string getConnectionString(string currentMethodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("..\\..\\..\\PersistenceLayer\\QueryStores\\ConnectionString.xml");

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Attributes["name"].Value.Equals(currentMethodName))
                {
                    this.connectionString = node.Attributes["value"].Value;
                    break;
                }
            }
            return this.connectionString;
        }

        public DataTable getSystemTreeViewData()
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            string sqlQueryString = "Select dbo.REPORTGetHierarchyPathByChildOid " +
                             "(x2.Oid,'SystemHierarchy') Path, x2.ItemName As PipeLine " +
                             "From JPipelineSystem x1 Join JNamedItem x2 on x1.oid = x2.oid";

            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, sqlConnection);

            sqlConnection.Open();

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable getWBSTreeViewData()
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            string sqlQueryString = "Select dbo.REPORTGetHierarchyPathByChildOid(x1.Oid,'WBSHierarchy') CompletePath From JWBSItem_CL x1";
            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable getDrawReportData()
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            string sqlQueryString = "Select dbo.REPORTGetHierarchyPathByChildOid " +
                          "(j1.Oid,'DrawingFolderHierarchy')Path,itemname As DrawingNo " +
                          "from JDDwgSnapIn2 j1 join XSnapInHasSheets x on x.OidDestination=j1.Oid join JNamedItem n on n.Oid=x.OidOrigin";

            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            dataTable.Rows.Add(@"Test Plant\Report\Test", "zip");
            sqlConnection.Close();
            return dataTable;
        }

        // Catalog Start
        public DataTable getCataTreeViewData()
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            string sqlQueryString = "Select itemName As rootPlant From JConfigProjectRoot x1 Join JNamedItem x2 on x1.oid=x2.oid";
            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable getCataDeppt()
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            string sqlQueryString = "Select Name From JDCatalogRoot x1 Order by Name";
            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable getPipeSpecList()
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            string sqlQueryString = "Select SpecName From JDPipeSpec_CL x1 order by SpecName";
            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable getSpecItems()
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            string sqlQueryString = @"Select distinct x1.ShortCode As PipePart From JDPipePartSpec_CL x1 
                                      Join XPipeSpecContainsPartSpecs x2 on x2.OidDestination=x1.Oid
                                      Join JDPipeSpec_CL x3 on x3.Oid=x2.OidOrigin
                                      --where x3.SpecName = 'JSR001'
                                      Order By PipePart";
            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            sqlConnection.Close();
            return dataTable;
        }


        // Catalog End
        public DataTable populateGVSystem(string items) //  PipeLine PipeRun Data Transfer From List To Gridview
        {
            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
            //    getConnectionString(new StackTrace().GetFrame(0).GetMethod().Name));

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
                this.connectionString);
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            //MessageBox.Show(item);
            DataTable plprdt = new DataTable();  
            SqlCommand cmd = new SqlCommand("Select x5.ItemName As PipeLine,x2.ItemName As PipeRun,NPD As Size, NPDUnitType As Unit,CASE WHEN ApprovalStatus =1 " +
                       "THEN 'Working' WHEN ApprovalStatus =2 THEN 'Review' WHEN ApprovalStatus =4 THEN 'Rejected' WHEN ApprovalStatus =8 THEN 'Approved' END AS Status, " +
                       "UserLogin,DateCreated,DateLastModified,x4.oid As PipeLine_OID,x1.oid As PipeRun_OID From JRtePipeRun x1 Join JNamedItem x2 on x1.oid=x2.oid Join " +
                       "XSystemHierarchy x3 on x2.oid=x3.OidDestination  Join JPipelineSystem x4 on x4.Oid=x3.OidOrigin  Join JNamedItem x5 on x5.Oid=x4.Oid Join JDObject " +
                       "x6 on x6.oid=x4.oid join JUserLogin x7 on x7.oid=UIDLastModifier where x5.ItemName IN (" + items + ")", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            plprdt.Load(reader);

            connection.Close();

            return plprdt;
        }

        public DataTable populateGVWBS(string items) //  WBS Data Transfer From List To Gridview
        {
            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
            //    getConnectionString(new StackTrace().GetFrame(0).GetMethod().Name));

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
                this.connectionString);
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            //MessageBox.Show(item);
            DataTable wbsdt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT x2.ItemName AS WBS_Item,CASE WHEN ApprovalStatus =1 THEN 'Working' WHEN ApprovalStatus =2 THEN 'Review' "+
                       "WHEN ApprovalStatus =4 THEN 'Rejected' WHEN ApprovalStatus =8 THEN 'Approved' END AS Status,DateCreated,DateLastModified,UserLogin,x1.Oid As WBS_OID "+
                       "FROM JWBSChild X1 Join JNamedItem X2 ON X2.Oid=X1.Oid Join JDObject x3 on x3.Oid = x1.Oid "+
                       "Join JUserLogin x4 on x4.oid = x3.UIDLastModifier where X2.ItemName IN (" + items + ")", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            wbsdt.Load(reader);

            connection.Close();

            return wbsdt;
        }

        public DataTable populateGVDrawingRpt(string items) //  Drawing & Reports Data Transfer From List To Gridview
        {          
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this.connectionString);
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
           
            DataTable drawdt = new DataTable();  
            SqlCommand cmd = new SqlCommand("SELECT X3.ItemName As DrawingNo,CASE WHEN ApprovalStatus =1 THEN 'Working' WHEN ApprovalStatus =2 THEN 'Review' WHEN ApprovalStatus =4 "+
                      "THEN 'Rejected' WHEN ApprovalStatus =8 THEN 'Approved' END AS Status, DateCreated,DateLastModified,UserLogin,DrawnBy,CheckedBy,ApprovedBy,FileSize "+
                      "FROM JDDwgPropertyObject X1 JOIN XSheetHasProperty X2 ON X1.Oid = X2.OidOrigin JOIN JNamedItem X3 ON X2.OidDestination = X3.Oid "+
                      "JOIN XSnapInHasSheets X4 ON X4.OidOrigin = X2.OidDestination JOIN JNamedItem X5 ON X5.oid = X4.OidDestination JOIN JDObject X6 on x6.Oid = X3.Oid " +
                      "JOIN JUserLogin X7 on X7.oid=UIDLastModifier where X3.ItemName IN (" + items + ")", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            
            drawdt.Load(reader);
            connection.Close();
            return drawdt;
        }

        public  DataTable populateBlobData(string selecteditem)
        {
            SqlConnection connection = new SqlConnection("Data Source= .\\SQLEXPRESS;" +
                " Initial Catalog=TestPlant_MDB;integrated security=SSPI;");
            
            connection.Open();
            DataTable dataTable = new DataTable();
            String query = "Select x1.ItemName As DrawingName, x1.ItemName + " +
                        "'.zip' as Drawing, d1.DataBlob As BlobData " + " From DRAWNGDocumentData d1"
                        + " join XMgrHasGeneratedDocuments x2 on x2.oidDestination = d1.oid"
                        + " join XObjectHasOutputView x4 on x4.oidOrigin = x2.oidOrigin  " +
                        "join JNamedItem x1 on x1.oid = x4.oidDestination " +
                        "where x1.Itemname = " + selecteditem;
            SqlCommand cmd = new SqlCommand(query, connection);

          // String tempQuery = "select 'zip', FileName, DataBlob from DRAWNGDocumentData where FileName='zip.zip';";
           //cmd = new SqlCommand(tempQuery, connection);

            SqlDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);
            connection.Close();
            return dataTable;
        }

        public DataTable populateCatalogGridView(string query) 
        {
            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
            //    getConnectionString(new StackTrace().GetFrame(0).GetMethod().Name));

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
                this.connectionString);
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            //MessageBox.Show(item);
            DataTable catalogdt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            catalogdt.Load(reader);

            connection.Close();

            return catalogdt;
        }

        public DataTable populateActiveRelationship() //  
        {
            string query = "Select x1.UserName As Package,rel_naming.UserName,DbView.DBViewName " +
                "From IJRelationDef AS rel " +
                "INNER JOIN IJRelationCollectionDef AS col1 ON rel.oid = col1.RelationGUID AND col1.IsOrigin = 1 " +
                "INNER JOIN IJRelationCollectionDef AS col2 ON rel.RelationGUID = col2.RelationGUID AND col2.IsOrigin = 0 " +
                "INNER JOIN IJNamedObject AS col1_naming ON col1.oid = col1_naming.oid " +
                "INNER JOIN IJNamedObject AS col2_naming ON col2.oid = col2_naming.oid " +
                "INNER JOIN IJNamedObject AS rel_naming ON rel.oid = rel_naming.oid " +
                "INNER JOIN JPackage_Has_JMembers AS cross_table ON rel.oid = cross_table.oidDst " +
                "INNER JOIN IJPackage AS package ON package.oid = cross_table.oidOrg " +
                "INNER JOIN IJDBView As DbView ON  DBView.oid = rel_naming.oid " +
                "INNER JOIN IJNamedObject AS x1 ON x1.oid = package.oid " +
                "Order By x1.UserName,rel_naming.UserName,DbView.DBViewName";

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "XENON",
                InitialCatalog = "AST2_CDB_SCHEMA",
                IntegratedSecurity = true
            };            
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            //MessageBox.Show(item);
            DataTable catalogdt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            catalogdt.Load(reader);
            connection.Close();
            return catalogdt;
        }

        public DataTable populateCodeListValues()
        {
            string accessDBPath = Path.GetFullPath(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\PersistenceLayer\AccessDatabase\CodeListViewData.accdb"));
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ accessDBPath+"; Persist Security Info=false;";
            string strSQL = "SELECT * FROM CodeListView";
            // Create a connection  
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // Create a command and set its connection  
                OleDbCommand command = new OleDbCommand(strSQL, connection);
                // Open the connection and execute the select command.  
                try
                {
                    // Open connecton  
                    connection.Open();
                    // Execute command  
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        DataTable dtCodeListValues = new DataTable();
                        dtCodeListValues.Load(reader);
                        return dtCodeListValues;
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
    }
}
