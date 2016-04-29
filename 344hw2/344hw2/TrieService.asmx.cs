using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace _344hw2
{
    /// <summary>
    /// Summary description for TrieService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TrieService : System.Web.Services.WebService
    {
        public static Trie search;

        [WebMethod]
        public void BuildTrie()
        {

        }

        [WebMethod]
        public void DownloadWiki()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString2"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("something");
            CloudBlockBlob blobby = container.GetBlockBlobReference("helloblob.txt");
            string text = blobby.DownloadText();

            return text;
        }

        [WebMethod]
        public List<string> SearchTrie(string searchString)
        {
            return new List<string>();
        }
    }
}
