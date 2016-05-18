using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;
using QuizLibrary;
using System.Security.Cryptography;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(

               ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("sumresults");
            table.CreateIfNotExists();

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("sumlist");
            queue.CreateIfNotExists();

            //gets or creates counter
            TableQuery<QuizEntity> counterquery = new TableQuery<QuizEntity>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "system.counter"));
            List<QuizEntity> bean = table.ExecuteQuery(counterquery).ToList();
            QuizEntity counter;
            if (bean == null || bean.Count == 0)
            {
                counter = new QuizEntity("0", "system.counter");
                table.Execute(TableOperation.InsertOrReplace(counter));
            } else
            {
                counter = bean.ElementAt(0);
            }

            //do work loop
            while (true)
            {
                CloudQueueMessage message = queue.GetMessage();
                if (message != null)
                {
                    Thread.Sleep(100);

                    //compute the password
                    string username = message.AsString;
                    byte[] salty = System.Text.Encoding.ASCII.GetBytes(username + "info344");
                    byte[] password = new MD5Cng().ComputeHash(salty);

                    //insert password and username
                    QuizEntity result = new QuizEntity(username, System.Text.Encoding.Default.GetString(password));
                    TableOperation insertOperation = TableOperation.InsertOrReplace(result);
                    table.Execute(insertOperation);

                    //delete task from queue
                    queue.DeleteMessage(message);

                    //increment counter
                    bean = table.ExecuteQuery(counterquery).ToList();
                    counter = bean.ElementAt(0);
                    int increment = Int32.Parse(counter.Username) + 1;
                    counter = new QuizEntity(increment + "", "system.counter");
                    table.Execute(TableOperation.InsertOrReplace(counter));
                }

            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
