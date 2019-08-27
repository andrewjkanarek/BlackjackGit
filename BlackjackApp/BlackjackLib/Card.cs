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

        public void ToggleAceValue()
        {
            // if it's not an ace, value cannot be toggled
            if (name != CardName.ACE) return;

            // toggle between values 11 and 1
            value = (value == 1) ? 11 : 1;
        }
    }

}
