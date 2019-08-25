using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Card
    {

        private CardName name;
        private int value;

        public CardName Name { get { return name; } }
        public int Value { get { return value; } }

        public Card(CardName name)
        {
            this.name = name;
            this.value = Deck.CardValDict[name];
        }
    }

}
