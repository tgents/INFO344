using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for statsearch
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class statsearch : System.Web.Services.WebService
    {
        [WebMethod]
        public void InsertTable()
        {
            string filename = System.Web.HttpContext.Current.Server.MapPath(@"/2015-2016.nba.stats.csv");
            List<string> filedata = LoadCSVFile(filename);
            var nbaplayers = filedata.Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new NBAPlayerStats(x[0], x[21]))
                .Take(30)
                .ToArray();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("nbaplayerstats");
            table.CreateIfNotExists();

            foreach(NBAPlayerStats player in nbaplayers)
            {
                TableOperation insertOperation = TableOperation.Insert(player);
                table.Execute(insertOperation);
            }
        }

        private List<string> LoadCSVFile(string filename)
        {
            List<string> result = new List<string>();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                while(!sr.EndOfStream)
                {
                    result.Add(sr.ReadLine());
                }
            }
            return result;
        }

        [WebMethod]
        public List<string> ReadTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("nbaplayerstats");

            TableQuery<NBAPlayerStats> rangeQuery = new TableQuery<NBAPlayerStats>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThan, "A"),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.LessThan, "Z"))
                 );
            List<string> stuff = new List<string>();
            foreach(NBAPlayerStats entity in table.ExecuteQuery(rangeQuery))
            {
                stuff.Add(entity.Name + " | " + entity.PPG);
            }
            return stuff;
        }

        [WebMethod]
        public void AddMessage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("myurls");
            queue.CreateIfNotExists();

            CloudQueueMessage message = new CloudQueueMessage("http://www.cnn.com/index.html");
            queue.AddMessage(message);
        }
        [WebMethod]
        public void DeleteMessage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("myurls");

            CloudQueueMessage message = queue.GetMessage(TimeSpan.FromMinutes(5));
            queue.DeleteMessage(message);
        }
    }
}
