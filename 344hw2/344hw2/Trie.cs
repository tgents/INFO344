using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _344hw2
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
            foreach(char letter in str)
            {
                int index = letter - 'a';
                if(index < 0)
                {
                    index = 26;
                }
                if(current.children == null)
                {
                    current.children = new Node[27];
                }
                if(current.children[index] == null)
                {
                    current.children[index] = new Node(letter);
                }
                current = current.children[index];
                
            }
            current.isWord = true;
        }

        public List<string> Search(string str)
        {
            Node current = rootNode;
            List<string> matches = new List<string>();
            return matches;
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