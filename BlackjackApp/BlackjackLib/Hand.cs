using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Hand
    {
        public List<Card> cards;
        public int totalValue;
        public StatsBase stats;

        public Hand()
        {
            cards = new List<Card>();
            totalValue = 0;
            stats = new Stats();           
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
