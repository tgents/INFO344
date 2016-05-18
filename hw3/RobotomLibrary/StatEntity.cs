using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotomLibrary
{
    public class StatEntity : TableEntity
    {
        public StatEntity() { }

        public List<Uri> lastTen { get; set; }
        public float memUsage { get; set; }
        public float cpuUsage { get; set; }
        public int queuesize { get; set; }
        public int tablesize { get; set; }
        public int visitcount { get; set; }
        public int status { get; set; }
    }
}
