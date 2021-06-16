using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PersistenceLayer
{
    public class DatabaseHelper
    {

        private static String connectionString = "Data Source=XENON;Initial Catalog=TestPlant_RDB;Integrated Security=True";

        public static DataTable load_plant()  //// Plant Tree Function
        {
            DataTable dt = new DataTable();
            String SQL = "Select itemName As rootPlant From JConfigProjectRoot x1 Join JNamedItem x2 on x1.oid=x2.oid";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand (SQL, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter (cmd))
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
             return dt;
        }
        // PipeLine Function
        public static DataTable load_pipeLine()   // PipeLine Tree Function
        {
            DataTable dt = new DataTable();
            String SQL = "Select ItemName As cPipeLineSystem,x1.oid As cPipeLineOID From JPipelineSystem x1 Join JNamedItem x2 on x1.oid=x2.oid";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        //cmd.Parameters.AddWithValue("@cPipeLineSytem", perentid);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }                            
                    }
                }
            }
            catch  (Exception ex)
            {
                
            }
            return dt;
        }

        // PipeRun Function

        public static DataTable load_pipeRun(int parentId) // PipeRun Tree Function
        {
            DataTable dt = new DataTable();            
            String SQL = "Select x2.ItemName As cPipeRun,x5.ItemName As pline From JRtePipeRun x1 Join JNamedItem x2 on x1.oid=x2.oid Join XSystemHierarchy x3 on x2.oid=x3.OidDestination Join JPipelineSystem x4 on x4.Oid=x3.OidOrigin Join JNamedItem x5 on x5.Oid=x4.Oid  Join XSystemHierarchy x6 on x5.oid=x6.OidDestination Where x2.ItemName";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString)) 
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        cmd.Parameters.AddWithValue("pline", parentId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }




        public static DataTable lsttosql() // List View To GridView Data Transfer Function
        {
            DataTable dtlts = new DataTable();
            String SQL = "Select x5.ItemName As PipeLine,x2.ItemName As PipeRun,NPD As Size, NPDUnitType As Unit,ApprovalStatus As Status,UserLogin,DateCreated,DateLastModified,x4.oid As PipeLine_OID,x1.oid As PipeRun_OID From JRtePipeRun x1 Join JNamedItem x2 on x1.oid=x2.oid Join XSystemHierarchy x3 on x2.oid=x3.OidDestination  Join JPipelineSystem x4 on x4.Oid=x3.OidOrigin  Join JNamedItem x5 on x5.Oid=x4.Oid Join JDObject x6 on x6.oid=x4.oid join JUserLogin x7 on x7.oid=UIDLastModifier";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        //cmd.Parameters.AddWithValue("@cPipeLineSytem", perentid);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dtlts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dtlts;
        }



        public static DataTable GetPiperunReport()  // Report Functions 
        {
            DataTable dtpipeRunReport = new DataTable();
            String SQL = "Select x5.ItemName As PipeLine,x2.ItemName As PipeRun,NPD As Size, NPDUnitType As Unit,ApprovalStatus As Status,UserLogin,DateCreated,DateLastModified,x4.oid As PipeLine_OID,x1.oid As PipeRun_OID From JRtePipeRun x1 Join JNamedItem x2 on x1.oid=x2.oid Join XSystemHierarchy x3 on x2.oid=x3.OidDestination Join JPipelineSystem x4 on x4.Oid=x3.OidOrigin  Join JNamedItem x5 on x5.Oid=x4.Oid Join JDObject x6 on x6.oid=x4.oid join JUserLogin x7 on x7.oid=UIDLastModifier";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {                        
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dtpipeRunReport);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dtpipeRunReport;
        }


        public class S3D  // S3D Functions 
        {
            public const String rootPlant       = "rootPlant";
            public const String cAreaSystem     = "cAreaSystem";
            public const String cUnitSystem     = "cUnitSystem";
            public const String cGenericSystem  = "cGenericSystem";
            public const String cPipingSystem   = "cPipingSystem";
            public const String cPipeLineSystem = "cPipeLineSystem";
            public const String cPipeRun        = "cPipeRun";
            public const String cPipingPart     = "cPipingPart";
            public const String pline           = "pline";
            public const String PipeLine        = "PipeLine";
            public const String PipeRun         = "PipeRun";
            public const String pipeRunidtree   = "";
            public const String pipeLineidtree  = "";


        }

    }
}
