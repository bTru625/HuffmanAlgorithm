using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuffmanAlgorithm.Entities
{
    public class CharNode : Node
    {
        private char character;

        public CharNode(char character, int frequency)
        {
            this.character = character;
            base.SetFrequency(frequency);

            // CharNode will always be leaf nodes, never will have kids.  
            base.SetLChild(null);
            base.SetRChild(null);
        }

        public char GetCharacter()
        {
            return character;
        }

        public void SetCharacter(char character)
        {
            this.character = character;
        }

    }
}