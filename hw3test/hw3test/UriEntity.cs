using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RobotomLibrary
{
    public class UriEntity : TableEntity
    {
        public UriEntity(Uri uri, string title, DateTime date)
        {
            this.PartitionKey = uri.Host;
            this.RowKey = uri.AbsolutePath.GetHashCode().ToString();

            this.Site = uri.AbsoluteUri;
            this.Date = date.ToString();
            this.Title = title;
        }

        public UriEntity() { }

        public string Site { get; private set; }
        public string Date { get; private set; }
        public string Title { get; set; }
        public int count { get; set; }
    }
}
