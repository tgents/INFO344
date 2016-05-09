using BestLibrary;
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
    /// Summary description for admin
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class admin : System.Web.Services.WebService
    {

        [WebMethod]
        public string CalculateSumUsingWorkerRole(int a, int b, int c)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("sumlist");
            queue.CreateIfNotExists();

            CloudQueueMessage message = new CloudQueueMessage(a + "," + b + "," + c);
            queue.AddMessage(message);

            return "Added to queue.";
        }

        [WebMethod]
        public string ReadSumFromTableStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("sumresults");

            List<IntegerEntity> intlist = table.ExecuteQuery(new TableQuery<IntegerEntity>()).ToList();
            string results = "";
            foreach(IntegerEntity i in intlist)
            {
                results += i.Result + ",";
            }
            return results;
        }
    }
}
