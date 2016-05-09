using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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
    /// Summary description for QuizService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class QuizService : System.Web.Services.WebService
    {
        [WebMethod]
        public string SubmitPhrase(string phrase)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("ReverseWord");
            table.CreateIfNotExists();

            Phrase store = new Phrase(phrase, ReverseWords(phrase));

            TableOperation insertOperation = TableOperation.Insert(store);
            table.Execute(insertOperation);

            return store.Words + " added successfully.";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> GetAllPhrases()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("ReverseWord");

            List<Phrase> phraselist = table.ExecuteQuery(new TableQuery<Phrase>()).ToList();
            List<string> rows = new List<string>();
            foreach(Phrase phrase in phraselist)
            {
                rows.Add(phrase.Words + " | " + phrase.Reverse);
            }

            return rows;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ReverseWords(string words)
        {
            string[] list = words.Split(' ');
            string reversewords = "";
            foreach(string word in list)
            {
                reversewords = word + " " + reversewords;
            }
            return reversewords.Trim();
        }
    }
}
