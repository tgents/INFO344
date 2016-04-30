using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            Trie trie = BuildTrie();
            while (true)
            {
                Console.Write("Search: ");
                string input = Console.ReadLine();
                List<string> hi = trie.Search(input.ToLower());
                if(hi == null)
                {
                    Console.Write("no words found :(");
                }else
                {
                    foreach (string word in hi)
                    {
                        Console.Write(word + ",");
                    }
                }
                Console.WriteLine();
            }
            
            //StreamWriter writefile = new StreamWriter("C:/Users/tgents/Desktop/searchresults.txt");
            //writefile.WriteLine("SEARCH RETURNED--------------");
            //Console.Read();
        }

        private static PerformanceCounter memprocess = new PerformanceCounter("Memory", "Available MBytes");
        
        public static float GetAvailableMBytes()
        {
            float memUsage = memprocess.NextValue();
            return memUsage;
        }

        public static Trie BuildTrie()
        {
            float startMem = GetAvailableMBytes();
            Trie triehard = new Trie();
            string filepath = "C:/Users/tgents/Desktop/wikititles.txt";
            StreamReader reader = new StreamReader(filepath);
            int countLines = 0;
            string currentWord = "";
            float usedMem = 0;
            while (!reader.EndOfStream)
            {
                if(countLines % 1000 == 0)
                {
                    usedMem = startMem - GetAvailableMBytes();
                    if(usedMem > 1000)
                    {
                        break;
                    }
                }
                currentWord = reader.ReadLine();
                triehard.Add(currentWord.ToLower());
                countLines++;
            }

            Console.WriteLine("Trie complete...");
            Console.WriteLine("Finished on: " + currentWord);
            Console.WriteLine("Words added: " + countLines);
            Console.WriteLine("Memory used " + usedMem);

            return triehard;
        }
    }
}
