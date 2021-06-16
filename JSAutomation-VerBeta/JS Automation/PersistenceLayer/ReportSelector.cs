//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PersistenceLayer
{
    public class ReportSelectorDB
    {
        public class ReportSelectorData
        {
            public String DBName { get; set; }
            public String QueryName { get; set; }
            public String Query { get; set; }
        }

        public List<ReportSelectorData> readQueryStore()
        {
            List<ReportSelectorData> lstReportSelectorData = new List<ReportSelectorData>();
            using (StreamReader streamReader = new StreamReader("..\\..\\..\\PersistenceLayer\\QueryStores\\QueryStore.json"))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                var items = javaScriptSerializer.Deserialize<Dictionary<string, object>>(streamReader.ReadToEnd());

                System.Collections.ArrayList jArrayDBs = (System.Collections.ArrayList)items["databases"];

                foreach(Object obj in jArrayDBs)
                {
                    string dbName = (obj as Dictionary<string, object>)["name"].ToString();
                    foreach (object obj2 in (obj as Dictionary<string, object>)["queries"] as System.Collections.ArrayList)
                    {
                        ReportSelectorData reportSelectorData = new ReportSelectorData();
                        reportSelectorData.DBName = dbName;
                        reportSelectorData.QueryName = (obj2 as Dictionary<string, object>).Keys.FirstOrDefault();
                        reportSelectorData.Query = (obj2 as Dictionary<string, object>)[reportSelectorData.QueryName].ToString();

                        lstReportSelectorData.Add(reportSelectorData);
                    }

                }

                //var items = JsonConvert.DeserializeObject<Dictionary<string, object>>(streamReader.ReadToEnd());
                //var jArrayDBs = (IList<object>)javaScriptSerializer.Deserialize<object>(items["databases"].ToString());

                //foreach (JObject oObj in jArrayDBs)
                //{
                //    String name = oObj.GetValue("name").ToString();
                //    var Queries = (JArray)oObj.GetValue("queries");

                //    foreach (JObject jQuery in Queries)
                //    {
                //        var dictQuery = JsonConvert.DeserializeObject<Dictionary<String, Object>>(jQuery.ToString());
                //        ReportSelectorData reportSelectorData = new ReportSelectorData();
                //        reportSelectorData.DBName = name;
                //        reportSelectorData.QueryName = dictQuery.Keys.FirstOrDefault();
                //        reportSelectorData.Query = dictQuery[reportSelectorData.QueryName].ToString();

                //        lstReportSelectorData.Add(reportSelectorData);
                //    }
                //}
            }
            return lstReportSelectorData;
        }
    }
}
