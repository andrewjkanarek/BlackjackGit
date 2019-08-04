using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Card
    {
        public CardName name { get; }
        public int value { get; }

        public Card(CardName name, int value)
        {
            this.name = name;
            this.value = value;
        }
    }

}
