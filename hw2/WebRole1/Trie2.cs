using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class Trie2
    {
        public Trie2.Node rootNode;
        public Trie2()
        {
            rootNode = new Trie2.Node();
        }

        public void Add(string str)
        {
            Trie2.Node current = rootNode;
            int index = 0;
            while (current.children != null && index < str.Length)
            {
                int childIndex = GetNum(str.ElementAt(index));
                if (current.children[childIndex] == null)
                {
                    current.children[childIndex] = new Trie2.Node(str.ElementAt(index));
                }
                current = current.children[childIndex];
                index++;
            }

            if (index == str.Length)
            {
                current.isWord = true;
            }
            else
            {
                current.words.AddLast(str.Substring(index));
                if (current.words.Count > 20)
                {
                    current.changeToNodes();
                }
            }
        }

        public List<string> Search(string str)
        {
            Trie2.Node current = rootNode;
            int pos = 0;
            while (current.children != null && pos < str.Length)
            {
                if (current == null)
                {
                    return null;
                }
                char currentChar = str.ElementAt(pos);
                int index = GetNum(currentChar);
                current = current.children[index];
                pos++;
            }
            List<string> matches = new List<string>();
            if (pos < str.Length)
            {
                foreach (string word in current.words)
                {
                    string wordup = str.Substring(0, pos) + word;
                    if (wordup.Contains(str))
                    {
                        matches.Add(wordup);
                    }
                }
            }
            else
            {
                matches = GetMoreWords(current, matches, str);
            }
            return matches;
        }

        private List<string> GetMoreWords(Trie2.Node current, List<string> gimmeWords, string prefix)
        {
            if (gimmeWords.Count() > 100)
            {
                return gimmeWords;
            }
            if (current.isWord)
            {
                gimmeWords.Add(prefix + current.id);
            }
            if (current.children == null)
            {
                foreach (string word in current.words)
                {
                    gimmeWords.Add(prefix + word);
                }
            }
            else
            {
                foreach (Trie2.Node child in current.children)
                {
                    if (child != null)
                    {
                        gimmeWords = GetMoreWords(child, gimmeWords, prefix + current.id);
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
            public LinkedList<string> words;
            public char id { get; set; }
            public bool isWord { get; set; }

            public Node()
            {
                id = ' ';
                isWord = false;
                words = new LinkedList<string>();
                children = null;
            }

            public Node(char letter)
            {
                id = letter;
                isWord = false;
                words = new LinkedList<string>();
                children = null;
            }

            public void changeToNodes()
            {
                children = new Node[27];
                foreach (string word in words)
                {
                    char childId = word.ElementAt(0);
                    int index = GetNum(childId);
                    if (children[index] == null)
                    {
                        children[index] = new Node(childId);
                    }
                    if (word.Length > 1)
                    {
                        children[index].words.AddLast(word.Substring(1));
                    }
                    else
                    {
                        children[index].isWord = true;
                    }
                }
                words = null;
            }

            private int GetNum(char letter)
            {
                int num = letter - 'a';
                return num < 0 ? 26 : num;
            }
        }
    }
}