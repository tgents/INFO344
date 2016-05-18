using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using RobotomLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for RoboTom
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class RoboTom : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string start(string url)
        {
            if (!url.Equals("http://cnn.com"))
            {
                return "only works with http://cnn.com at the moment";
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue commandQ = queueClient.GetQueueReference("commandq");

            CloudQueueMessage command = new CloudQueueMessage(Robotom.COMMAND_START + " " + url);
            commandQ.AddMessage(command);

            return "Starting crawl...";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string stop()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue commandQ = queueClient.GetQueueReference("commandq");

            CloudQueueMessage command = new CloudQueueMessage(Robotom.COMMAND_STOP);
            commandQ.AddMessage(command);

            return "Stopping crawl...";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string clearTables()
        {
            stop();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable resultsTable = tableClient.GetTableReference("crawltable");
            CloudTable statsTable = tableClient.GetTableReference("stattable");
            CloudTable errorTable = tableClient.GetTableReference("errortable");
            try
            {
                resultsTable.Delete();
                statsTable.Delete();
                errorTable.Delete();
            }
            catch (Exception e)
            {
                return "Tables already deleted..." + e.Message;
            }

            return "Crawl stopped, Clearing tables...";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string createTables()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            try
            {
                CloudTable resultTable = tableClient.GetTableReference("crawltable");
                CloudTable errorTable = tableClient.GetTableReference("errortable");
                CloudTable statsTable = tableClient.GetTableReference("stattable");
                resultTable.CreateIfNotExists();
                statsTable.CreateIfNotExists();
                errorTable.CreateIfNotExists();
            }
            catch (Exception e)
            {
                return e.Message + "Please wait a least 1 minute before creating after deleting...";
            }

            return "Creating tables...";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<int> getStats()
        {
            List<int> stats = new List<int>();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable statsTable = tableClient.GetTableReference("stattable");

            TableQuery<StatEntity> counterquery = new TableQuery<StatEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "TomBot"));
            List<StatEntity> bean = statsTable.ExecuteQuery(counterquery).ToList();

            if (bean.Count != 0)
            {
                StatEntity getem = bean.ElementAt(0);
                stats.Add(Convert.ToInt32(getem.memUsage));
                stats.Add(Convert.ToInt32(getem.cpuUsage));
                stats.Add(getem.queuesize);
                stats.Add(getem.tablesize);
                stats.Add(getem.visitcount);
            }

            return stats;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> getLastTen()
        {
            List<string> uris = new List<string>();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable statsTable = tableClient.GetTableReference("stattable");

            TableQuery<StatEntity> counterquery = new TableQuery<StatEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "TomBot"));
            List<StatEntity> bean = statsTable.ExecuteQuery(counterquery).ToList();

            if (bean.Count != 0)
            {
                StatEntity getem = bean.ElementAt(0);
                if (getem.lastTen != null)
                {
                    foreach (Uri uri in getem.lastTen)
                    {
                        uris.Add(uri.AbsoluteUri);
                    }
                }
            }

            return uris;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getStatus()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable statsTable = tableClient.GetTableReference("stattable");

            TableQuery<StatEntity> counterquery = new TableQuery<StatEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "TomBot"));
            List<StatEntity> bean = statsTable.ExecuteQuery(counterquery).ToList();

            if (bean.Count != 0)
            {
                StatEntity getem = bean.ElementAt(0);
                if (getem.status == Robotom.STATUS_STOP)
                {
                    return "TomBot is stopped...";
                }
                else if (getem.status == Robotom.STATUS_IDLE)
                {
                    return "TomBot is idle...";
                }
                else if (getem.status == Robotom.STATUS_LOADING)
                {
                    return "TomBot is loading...";
                }
                else if (getem.status == Robotom.STATUS_CRAWLING)
                {
                    return "TomBot is crawling...";
                }
            }

            return "Could not retrieve status...";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> getErrors()
        {
            List<string> uris = new List<string>();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable errorTable = tableClient.GetTableReference("errortable");

            TableQuery<StatEntity> counterquery = new TableQuery<StatEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "TomBot"));
            List<UriEntity> bean = errorTable.ExecuteQuery(new TableQuery<UriEntity>()).ToList();

            if (bean.Count != 0)
            {
                foreach (UriEntity getem in bean)
                {
                    uris.Add(getem.Site.AbsoluteUri);
                }
            }

            return uris;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getTitle(string url)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch (Exception e)
            {
                return "Please enter a valid URL...";
            }
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable statsTable = tableClient.GetTableReference("crawltable");

            TableQuery<UriEntity> counterquery = new TableQuery<UriEntity>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, uri.Host),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, uri.AbsolutePath.GetHashCode().ToString())));

            List<UriEntity> bean = statsTable.ExecuteQuery(counterquery).ToList();

            if (bean.Count != 0)
            {
                return bean.ElementAt(0).Title;
            }

            return "Could not find URL...";
        }
    }
}
