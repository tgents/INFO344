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

            this.Site = uri;
            this.Date = date;
            this.Title = title;
        }

        public UriEntity() { }

        public Uri Site { get; private set; }
        public DateTime Date { get; private set; }
        public string Title { get; set; }
        public int count { get; set; }
    }
}
