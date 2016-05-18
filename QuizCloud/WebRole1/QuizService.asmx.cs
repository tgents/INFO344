using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using QuizLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for QuizService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class QuizService : System.Web.Services.WebService
    {

        [WebMethod]
        public string CreateAccountInWorkerRole(string username)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("sumlist");
            queue.CreateIfNotExists();

            CloudQueueMessage message = new CloudQueueMessage(username);
            queue.AddMessage(message);

            return "Username Added!";
        }

        [WebMethod]
        public string GetPasswordFromTableStorage(string username)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("sumresults");
            table.CreateIfNotExists();

            List<QuizEntity> userlist = table.ExecuteQuery(new TableQuery<QuizEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, username))).ToList();
            foreach (QuizEntity i in userlist)
            {
                if(i.Username == username)
                {
                    return i.Password;
                }
            }
            return "Error: Password not found";
        }

        [WebMethod]
        public int CountPasswords()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("sumresults");
            table.CreateIfNotExists();

            List<QuizEntity> bean = table.ExecuteQuery(new TableQuery<QuizEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "system.counter"))).ToList();

            int count = Int32.Parse(bean.ElementAt(0).Username);
            return count;
        }
    }
}
