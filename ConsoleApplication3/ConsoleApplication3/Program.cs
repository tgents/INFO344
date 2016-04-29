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
            Trie trie = new Trie();
            trie = BuildTrie(trie);
            List<string> hi = trie.Search("ba");
            //StreamWriter writefile = new StreamWriter(System.IO.Path.GetFullPath("appwritestuff.txt"));
            //writefile.WriteLine("SEARCH RETURNED--------------");
            foreach (string word in hi)
            {
                Console.WriteLine(word);
            }
            Console.Read();
        }

        private static PerformanceCounter memprocess = new PerformanceCounter("Memory", "Available MBytes");
        
        public static float GetAvailableMBytes()
        {
            float memUsage = memprocess.NextValue();
            return memUsage;
        }

        public static Trie BuildTrie(Trie triehard)
        {
            triehard = new Trie();
            string filepath = System.IO.Path.GetFullPath("wikititles.txt");
            StreamReader reader = new StreamReader(filepath);
            int countLines = 0;
            string currentWord = "";
            while (!reader.EndOfStream)
            {
                if (countLines % 1000 == 0)
                {
                    float mem = GetAvailableMBytes();
                    if (mem < 50)
                    {
                        break;
                    }
                }
                currentWord = reader.ReadLine();
                if (currentWord.StartsWith("C"))
                {
                    break;
                }
                //Console.WriteLine(currentWord);
                triehard.Add(currentWord.ToLower());
                countLines++;
            }
            return triehard;
        }
    }
}
