using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuffmanAlgorithm.Entities
{
    public class CharNode : Node
    {
        public char character;

        public CharNode(char character, int frequency)
        {
            this.character = character;
            this.frequency = frequency;

            // CharNode will always be leaf nodes, never will have kids.  
            lChild = null;
            rChild = null;
        }

        public char GetCharacter()
        {
            return character;
        }

    }
}