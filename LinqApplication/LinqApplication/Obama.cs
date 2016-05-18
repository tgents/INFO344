using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinqApplication
{
    class Obama
    {
        static void Main(string[] args)
        {
            string filepath = "C:/Users/Thomas/Dropbox/School/INFO344/obama.txt";
            StreamReader reader = new StreamReader(filepath);
            int obamaCount = 0;
            int presObamaCount = 0;
            while (!reader.EndOfStream)
            {
                string current = reader.ReadLine();
                string[] sentences = current.Split('.', '!', '?');
                var obamaSentences = sentences
                    .Select(x => x)
                    .Where(x => Regex.IsMatch(x, ".*[^a-z]obama[^a-z].*", RegexOptions.IgnoreCase))
                    .ToList();
                var presObamaSentences = sentences
                    .Select(x => x)
                    .Where(x => Regex.IsMatch(x, ".*[^a-z]obama[^a-z].*", RegexOptions.IgnoreCase)
                    && Regex.IsMatch(x, ".*[^a-z]president[^a-z].*", RegexOptions.IgnoreCase))
                    .ToList();

                

                obamaCount += obamaSentences.Count;
                presObamaCount += presObamaSentences.Count;
            }
            Console.WriteLine(obamaCount + " " + presObamaCount);
            Console.Read();
        }
    }
}
