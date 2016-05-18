using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotomLibrary
{
    public class Robotom
    {
        public static int STATUS_STOP = -1;
        public static int STATUS_IDLE = 0;
        public static int STATUS_LOADING = 1;
        public static int STATUS_CRAWLING = 2;
        public static string COMMAND_STOP = "stop";
        public static string COMMAND_START = "start";

        
    }
}
