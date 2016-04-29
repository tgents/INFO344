using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class Trie
    {
        public Node rootNode;
        public Trie()
        {
            rootNode = new Node();
        }

        public void Add(string str)
        {
            Node current = rootNode;
            foreach (char letter in str)
            {
                int index = GetNum(letter);
                if (current.children == null)
                {
                    current.children = new Node[27];
                }
                if (current.children[index] == null)
                {
                    current.children[index] = new Node(letter);
                }
                current = current.children[index];
            }
            current.isWord = true;
        }

        public List<string> Search(string str)
        {
            Node current = GetLastNode(rootNode, str, 0);
            if (current == null)
            {
                return null;
            }
            List<string> matches = new List<string>();
            matches = GetStrings(current, matches, str);
            return matches;
        }

        private Node GetLastNode(Node current, string str, int index)
        {
            if (current == null || index == str.Length)
            {
                return current;
            }
            int num = GetNum(str.ElementAt(index));
            return GetLastNode(current.children[num], str, index + 1);
        }

        private List<string> GetStrings(Node current, List<string> gimmeWords, string previous)
        {
            if (gimmeWords.Count() < 100 && current.children != null)
            {
                if (current.isWord)
                {
                    gimmeWords.Add(previous);
                }
                foreach (Node child in current.children)
                {
                    if (child != null)
                    {
                        gimmeWords = GetStrings(child, gimmeWords, previous + child.id);
                    }
                }
            }
            return gimmeWords;
        }

        private int GetNum(char letter)
        {
            int num = letter - 'a';
            return num < 0 ? 26 : num;
        }

        public class Node
        {
            public Node[] children { get; set; }
            //public LinkedList<string> words;
            public char id { get; set; }
            public bool isWord { get; set; }

            public Node()
            {
                id = ' ';
            }

            public Node(char letter)
            {
                id = letter;
                isWord = false;
            }
        }
    }
}
