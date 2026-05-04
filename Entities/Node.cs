namespace HuffmanAlgorithm.Entities
{
    public class Node
    {
        private int frequency;
        private Node? parent;
        private Node? lChild;
        private Node? rChild;

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

        public void SetFrequency(int frequency)
        {
            this.frequency = frequency;
        }
        public void SetParent(Node? parent)
        {
            this.parent = parent;
        }
        public void SetLChild(Node? lChild)
        {
            this.lChild = lChild;
        }
        public void SetRChild(Node? rChild)
        {
            this.rChild = rChild;
        }

    }
}