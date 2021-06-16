using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace PersistenceLayer
{
    public class PropertyUpdateDB
    {
        public class UpdateStatusFields
        {
            public string PipeLine { get; set; }
            public string PipeRun { get; set; }
            public string PipeLine_OID { get; set; }

        }


        SqlConnection con = new SqlConnection(@"Data Source=XENON;Initial Catalog=TestPlant_MDB;Integrated Security=True");

        public void updateProperty(Dictionary<string, UpdateStatusFields>.KeyCollection keyCollection,
            string codlistvalue)
        {
            try
            {
                con.Open();
                foreach (string ploid in keyCollection)
                {
                    SqlCommand cmdupdate = new SqlCommand(
                        "Update JDObject SET ApprovalStatus = @ast, ApprovalReason= @ar " +
                        "Where JDObject.oid = @ploid", con);
                    cmdupdate.Parameters.AddWithValue("ast", codlistvalue);
                    cmdupdate.Parameters.AddWithValue("ar", codlistvalue);
                    cmdupdate.Parameters.AddWithValue("ploid", ploid);
                    cmdupdate.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}
