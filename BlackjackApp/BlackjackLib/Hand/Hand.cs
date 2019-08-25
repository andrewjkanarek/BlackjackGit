using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Hand
    {
        #region Public Members

        public List<Card> cards;
        public int totalValue;

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
            totalValue += card.value;
        }

        #endregion



    }
}
