using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.WebService1SoapClient client = new ServiceReference1.WebService1SoapClient();
            int[] odds = client.OddNumbers(300);

            foreach(int i in odds)
            {
                Console.WriteLine(i);
            }
            Console.Read();
        }
    }
}
