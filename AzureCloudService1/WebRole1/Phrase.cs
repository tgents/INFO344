using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class Phrase : TableEntity
    {
        public Phrase(string words, string reverse)
        {
            this.PartitionKey = words;
            this.RowKey = Guid.NewGuid().ToString();

            this.Words = words;
            this.Reverse = reverse;
        }

        public Phrase() { }

        public string Words { get; set; }
        public string Reverse { get; set; }
    }
}