using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public abstract class HandBase
    {
        #region Public Members

        public List<Card> cards;
        public int totalValue;

        #endregion

        #region Constructor

        public HandBase()
        {
            cards = new List<Card>();
            totalValue = 0;
        }

        #endregion

        #region Public Functions

        public virtual void AddCard(Card card)
        {
            AddCardHelper(card);
        }

        #endregion

        #region Protected Functions

        protected virtual void AddCardHelper(Card card)
        {
            cards.Add(card);
            totalValue += card.value;
        }

        #endregion

    }
}
