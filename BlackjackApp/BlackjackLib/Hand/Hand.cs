using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Hand
    {
        #region Public Members

        private List<Card> cards;
        private int totalValue;

        public List<Card> Cards { get { return cards; } }
        public int TotalValue { get { return totalValue; } }

        #endregion

        #region Constructor

        public Hand()
        {
            cards = new List<Card>();
            totalValue = 0;
        }

        #endregion

        #region Public Functions

        public void AddCard(Card card)
        {
            cards.Add(card);
            totalValue += card.Value;
        }

        public Hand Clone()
        {
            return new Hand
            {
                cards = new List<Card>(this.cards),
                totalValue = this.totalValue
            };
        }

        #endregion



    }
}
