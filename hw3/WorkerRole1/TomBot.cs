using HtmlAgilityPack;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using RobotomLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WorkerRole1
{
    public class TomBot
    {
        private HashSet<Uri> disallow;
        public HashSet<Uri> visited { get; private set; }

        private CloudQueue htmlQ;
        private CloudTable resultTable;
        private CloudTable errorTable;

        public int queueCount { get; set; }
        public int tableCount { get; set; }

        public Queue<Uri> lastTen { get; private set; }
        public long timer { get; private set; }

        public TomBot(CloudQueue htmlqueue, CloudTable results, CloudTable errors, int resultscount)
        {
            disallow = new HashSet<Uri>();
            visited = new HashSet<Uri>();
            lastTen = new Queue<Uri>();
            htmlQ = htmlqueue;
            resultTable = results;
            errorTable = errors;
            htmlQ.CreateIfNotExists();
            timer = 0;

            queueCount = 0;
            tableCount = resultscount;
        }

        //return 1 for success, 0 for fail
        public int ParseHtml(Uri uri)
        {


            if (isDisallowed(uri))
            {
                return 0;
            }

            long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string sitedata = "";
            try
            {
                WebClient downloader = new WebClient();
                sitedata = downloader.DownloadString(uri);
            }catch(Exception e)
            {
                UriEntity error = new UriEntity(uri, e.Message, DateTime.Now);
                errorTable.ExecuteAsync(TableOperation.Insert(error));
                visited.Add(uri);
                queueCount--;
                return 0;
            }
            
            string hi = uri.AbsoluteUri;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(sitedata);

            HtmlNodeCollection hrefs = doc.DocumentNode.SelectNodes("//a[@href]");

            if(hrefs == null)
            {
                return 0;
            }

            foreach (HtmlNode node in hrefs)
            {
                var href = node.Attributes["href"];
                string url = href.Value;
                if (url.StartsWith("/") && !url.StartsWith("//"))
                {
                    htmlQ.AddMessageAsync(new CloudQueueMessage("http://" + uri.Host + url));
                    queueCount++;
                }
                else if (url.StartsWith("http://bleacherreport.com/articles"))
                {
                    htmlQ.AddMessageAsync(new CloudQueueMessage(url));
                    queueCount++;
                }
            }

            long stop = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            timer = stop - start;

            //get title
            HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//title");
            string title = "";
            if (titleNode != null)
            {
                title = titleNode.InnerText;
            }

            //get date
            HtmlNode lastmod = doc.DocumentNode.SelectSingleNode("//meta[@name='lastmod']");
            string date = "";
            if (lastmod != null)
            {
                date = lastmod.GetAttributeValue("content", "");
            }

            DateTime converteddate = date.Equals("") ? new DateTime() : Convert.ToDateTime(date);

            UriEntity insert = new UriEntity(uri, title, converteddate);

            try
            {
                if (title.Contains("Error"))
                {
                    errorTable.ExecuteAsync(TableOperation.Insert(insert));
                }
                else
                {
                    if (!visited.Contains(uri))
                    {
                        resultTable.ExecuteAsync(TableOperation.Insert(insert));
                        tableCount++;

                        lastTen.Enqueue(uri);
                        if (lastTen.Count > 10)
                        {
                            lastTen.Dequeue();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            visited.Add(uri);
            queueCount--;

            return 1;
        }

        //return 1 for success, 0 for fail
        public int ParseRobot(Uri uri)
        {
            var webRequest = WebRequest.Create(@uri.AbsoluteUri);

            var response = webRequest.GetResponse();
            var content = response.GetResponseStream();
            var reader = new StreamReader(content);
            Uri root = new Uri("http://cnn.com");

            List<Uri> sitemaps = new List<Uri>();

            if (uri.AbsolutePath.StartsWith("http://cnn.com"))
            {
                sitemaps.Add(new Uri("http://bleacherreport.com/sitemap/nba.xml"));
            }

            while (!reader.EndOfStream)
            {
                string current = reader.ReadLine();
                string url = current.Substring(current.IndexOf(' '));
                if (current.StartsWith("sitemap", StringComparison.OrdinalIgnoreCase))
                {
                    sitemaps.Add(new Uri(url));
                }
                else if (current.StartsWith("disallow", StringComparison.OrdinalIgnoreCase))
                {
                    disallow.Add(new Uri(root, url));
                }
            }
            parseSiteMaps(sitemaps);
            return 1;
        }

        private void parseSiteMaps(List<Uri> sitemaps)
        {
            while (sitemaps.Count > 0)
            {
                Uri next = sitemaps.ElementAt(0);
                sitemaps.AddRange(parseXml(next));
                sitemaps.Remove(next);
            }
        }

        private List<Uri> parseXml(Uri uri)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(uri.AbsoluteUri);
            string childname = xmldoc.LastChild.Name;
            DateTime compare = Convert.ToDateTime("2016-04-01");
            List<Uri> newSitemaps = new List<Uri>();
            if (childname.Equals("sitemapindex") || childname.Equals("urlset"))
            {
                foreach (XmlNode sitemap in xmldoc.LastChild.ChildNodes)
                {
                    string url = "";
                    string date = DateTime.Now.ToString();
                    foreach (XmlNode info in sitemap.ChildNodes)
                    {
                        if (info.Name.Equals("loc"))
                        {
                            url = info.InnerText;
                        }
                        else if (info.Name.Equals("lastmod"))
                        {
                            date = info.InnerText;
                        }
                    }

                    if (!date.Equals("") && Convert.ToDateTime(date).CompareTo(compare) >= 0)
                    {
                        if (url.EndsWith(".xml"))
                        {
                            newSitemaps.Add(new Uri(url));
                        }
                        else
                        {
                            htmlQ.AddMessageAsync(new CloudQueueMessage(url));
                            queueCount++;
                        }
                    }
                }
            }
            return newSitemaps;
        }

        private bool isDisallowed(Uri uri)
        {
            foreach (Uri no in disallow)
            {
                if (uri.AbsoluteUri.Contains(no.AbsoluteUri))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
