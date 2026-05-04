using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuffmanAlgorithm.Entities;

namespace HuffmanAlgorithm.Services
{
    public class Service
    {

        public Dictionary<char, int> BuildFrequencySet(string original)
        {

            if (string.IsNullOrEmpty(original))
            {
                throw new InvalidOperationException
                    ("Input string was null or empty when trying to build the frequency set.");
            }


            Dictionary<char, int> frequencySet = new Dictionary<char, int>();

            foreach (char c in original)
            {
                if (frequencySet.ContainsKey(c))
                {
                    frequencySet[c]++;
                }
                else
                {
                    frequencySet[c] = 1;
                }
            }
            return frequencySet;
        }

        public PriorityQueue<Node, int> BuildFrequencyQueue(Dictionary<char, int> frequencySet)
        {
            if (frequencySet == null || frequencySet.Count == 0)
            {
                throw new InvalidOperationException
                    ("Frequency set was empty / null when trying to build the frequency queue.");
            }

            PriorityQueue<Node, int> frequencyQueue = new PriorityQueue<Node, int>();

            foreach (KeyValuePair<char, int> entry in frequencySet)
            {
                CharNode charNode = new CharNode(entry.Key, entry.Value);

                frequencyQueue.Enqueue(charNode, charNode.GetFrequency());
            }


            return frequencyQueue;
        }

        // will modify the frequency queue until there is only one node left, also adding the parents and children, effectively making a Double Linked Tree, and calling the last remaining node, will be the root.
        public Node TurnPriorityQueueIntoTree(PriorityQueue<Node, int> frequencyQueue)
        {
            if (frequencyQueue.Count == 0)
            {
                throw new InvalidOperationException
                    ("Frequency queue was empty when trying to build the tree.");
            }

            while (frequencyQueue.Count > 1)
            {
                Node left = frequencyQueue.Dequeue();
                Node right = frequencyQueue.Dequeue();

                // this is a Node, not a CharNode!
                Node parent = new Node(left.GetFrequency() + right.GetFrequency())
                {
                    lChild = left,
                    rChild = right
                };

                parent.lChild.parent = parent;
                parent.rChild.parent = parent;

                frequencyQueue.Enqueue(parent, parent.GetFrequency());
            }

            return frequencyQueue.Dequeue();
        }

        public Dictionary<char, List<bool>> BuildCypherTable(Node root)
        {
            if (root == null)
            {
                throw new InvalidOperationException
                    ("Root Node was null when trying to build the cypher table.");
            }

            Dictionary<char, List<bool>> cypherTable = new Dictionary<char, List<bool>>();

            void Traverse(Node node, List<bool> path)
            {
                if (node is CharNode charNode)
                {
                    cypherTable[charNode.GetCharacter()] = new List<bool>(path);
                }
                else
                {
                    if (node.GetLChild() != null)
                    {
                        path.Add(false);
                        Traverse(node.GetLChild(), path);
                        path.RemoveAt(path.Count - 1);
                    }
                    if (node.GetRChild() != null)
                    {
                        path.Add(true);
                        Traverse(node.GetRChild(), path);
                        path.RemoveAt(path.Count - 1);
                    }
                }
            }

            Traverse(root, new List<bool>());
            return cypherTable;
        }

        public List<List<bool>> EncodeString(string original, Dictionary<char, List<bool>> cypherTable)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new InvalidOperationException
                    ("Input string was null or empty when trying to encode the string.");
            }
            if (cypherTable == null || cypherTable.Count == 0)
            {
                throw new InvalidOperationException
                    ("Cypher table was empty / null when trying to encode the string.");
            }

            List<List<bool>> encodedString = new List<List<bool>>();

            foreach (char c in original)
            {
                if (cypherTable.ContainsKey(c))
                {
                    encodedString.Add(cypherTable[c]);
                }
                else
                {
                    throw new InvalidOperationException
                        ($"Character '{c}' not found in cypher table when trying to encode the string.");
                }
            }

            return encodedString;
        }

        public string DecodeString(List<List<bool>> encodedString, Dictionary<char, List<bool>> cypherTable)
        {
            if (encodedString == null || encodedString.Count == 0)
            {
                throw new InvalidOperationException
                    ("Encoded string was empty / null when trying to decode the string.");
            }
            if (cypherTable == null || cypherTable.Count == 0)
            {
                throw new InvalidOperationException
                    ("Cypher table was empty / null when trying to decode the string.");
            }

            string decodedString = "";

            foreach (List<bool> cypherChar in encodedString)
            {
                var match = cypherTable.FirstOrDefault(
                    entry => entry.Value.SequenceEqual(cypherChar));

                if (match.Value != null)
                {
                    decodedString += match.Key;
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Cypher '[{string.Join(",", cypherChar.Select(b => b ? "1" : "0"))}]' not found in cypher table."
                    );
                }
            }

            return decodedString;
        }


    }
}