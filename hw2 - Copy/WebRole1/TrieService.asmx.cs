using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for TrieService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TrieService : System.Web.Services.WebService
    {
        public static Trie triehard;
        private PerformanceCounter memprocess = new PerformanceCounter("Memory", "Available MBytes");

        [WebMethod]
        public bool CheckTrie()
        {
            return triehard == null;
        }

        [WebMethod]
        public float GetAvailableMBytes()
        {
            float memUsage = memprocess.NextValue();
            return memUsage;
        }

        [WebMethod]
        public void RemoveTrie()
        {
            triehard = null;
        }

        [WebMethod]
        public string BuildTrie()
        {
            RemoveTrie();
            triehard = new Trie();
            string filepath = System.Web.HttpContext.Current.Server.MapPath(@"/wikititles.txt");
            StreamReader reader = new StreamReader(filepath);
            float startMem = GetAvailableMBytes();
            float memUsed = 0;
            int countLines = 0;
            string currentWord = "";
            while (!reader.EndOfStream)
            {
                if (countLines % 1000 == 0)
                {
                    float mem = GetAvailableMBytes();
                    memUsed = startMem - mem;
                    if(mem < 50)
                    {
                        break;
                    }
                }
                currentWord = reader.ReadLine();
                countLines++;
                triehard.Add(currentWord.Trim().ToLower());
            }
            return "Last insert: "+ currentWord + ", Lines: " + countLines + ", MemUsed: " + memUsed;
        }

        [WebMethod]
        public string DownloadWiki()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("something");
            CloudBlockBlob blobby = container.GetBlockBlobReference("wikititles2.txt");
            string filepath = System.Web.HttpContext.Current.Server.MapPath(@"/wikititles.txt");
            blobby.DownloadToFile(filepath, System.IO.FileMode.Create);
            return "success";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchTrie(string searchString)
        {
            if(triehard == null)
            {
                DownloadWiki();
                BuildTrie();
                return new JavaScriptSerializer().Serialize("Service is rebooting...");
            }
            return new JavaScriptSerializer().Serialize(triehard.Search(searchString));
        }
    }
}
