using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using HuffmanAlgorithm.Entities;

namespace HuffmanAlgorithm.Services
{
    public class Service
    {

        // COMPLEXITY = O(n) 
        // One loop through no matter how long the string is. 
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

        // COMPLEXITY = O(n) 
        // One loop through no matter how long the string is.
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

        // COMPLEXITY = O(n log n)
        // The while loop runs n-1 times, and each iteration involves dequeuing and enqueuing nodes, which is O(log n).
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
                Node parent = new Node(left.GetFrequency() + right.GetFrequency());

                parent.SetLChild(left);
                parent.SetRChild(right);

                parent.GetLChild().SetParent(parent);
                parent.GetRChild().SetParent(parent);

                frequencyQueue.Enqueue(parent, parent.GetFrequency());
            }

            return frequencyQueue.Dequeue();
        }

        // COMPLEXITY = O(n log n)
        // Getting to any leaf node is O(log n) in a balanced tree, and we need to visit each leaf node to build the cypher table, which is O(n) in the worst case. 
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



            // Sort the cypher table by the bit length of the cypher.

            // IDK how the compiler sorts this, so I dont want to speculate on the complexity, but I know that it is at least O(n log n) because sorting doesnt get faster than that. 

            cypherTable = cypherTable.OrderBy(entry => entry.Value.Count)
                                    .ToDictionary(entry => entry.Key, entry => entry.Value);

            return cypherTable;
        }

        // COMPLEXITY = O(n) 
        // One loop through no matter how long the string is.
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

        // COMPLEXITY = O(n) 
        // One loop through no matter how long the string is.
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