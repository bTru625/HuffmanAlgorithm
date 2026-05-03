using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuffmanAlgorithm.Entities
{
    public class Node
    {
        public int frequency;
        public Node? parent;
        public Node? lChild;
        public Node? rChild;

        public Node()
        {
            frequency = 0;
            parent = null;
            lChild = null;
            rChild = null;
        }

        public Node(int frequency)
        {
            this.frequency = frequency;
            parent = null;
            lChild = null;
            rChild = null;
        }

        public Node(int frequency, Node? parent)
        {
            this.frequency = frequency;
            this.parent = parent;
            lChild = null;
            rChild = null;
        }

        public int GetFrequency()
        {
            return frequency;
        }

        public Node? GetParent()
        {
            return parent;
        }

        public Node? GetLChild()
        {
            return lChild;
        }

        public Node? GetRChild()
        {
            return rChild;
        }
    }
}