using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        public static int counter = 0;
        [WebMethod]
        public string HelloWorld(string name)
        {
            return "hello world" + name + counter++;
        }

        [WebMethod]
        public string ReadFile()
        {
            string file = HttpContext.Current.Server.MapPath("~/output.txt");
            //string file = HttpRuntime.AppDomainAppPath + "output.txt";
            using (StreamWriter sw = new StreamWriter(file))
            {
                for(int i = 0; i < 10; i++)
                {
                    sw.WriteLine(i);
                }
            }
            string line = "";
            using (StreamReader sr = new StreamReader(file))
            {
                
                while (sr.EndOfStream == false)
                {
                    line += sr.ReadLine();
                }
            }
            return line;
        }

        [WebMethod]
        public string GimmeCode()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"];
        }

        [WebMethod]
        public int[] OddNumbers(int n)
        {
            List<int> oddNums = new List<int>();

            for(int i = 0; i < n; i++)
            {
                if (i % 2 == 1)
                {
                    oddNums.Add(i);
                }
            }

            return oddNums.ToArray();
        }

        [WebMethod]
        public string GetBlobThing()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString2"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("something");
            CloudBlockBlob blobby = container.GetBlockBlobReference("helloblob.txt");
            string text = blobby.DownloadText();

            return text;
        }
    }
}
