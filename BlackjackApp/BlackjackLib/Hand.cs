using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Hand
    {
        public List<Card> cards;
        public int totalValue;
        public Stats beforeHit;
        public Stats afterHit;

        public Hand()
        {
            cards = new List<Card>();
            totalValue = 0;
            beforeHit = new Stats();
            afterHit = new Stats();
        }
        
        public void AddCard(Card card)
        {
            cards.Add(card);
            totalValue += card.value;
        }

        public int GetTotalValue()
        {
            return totalValue;
        }
    }
}
