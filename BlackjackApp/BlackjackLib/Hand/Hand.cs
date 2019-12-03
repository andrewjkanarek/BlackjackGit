using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackLib
{
    public class Hand
    {
        #region Public Members

        public List<Card> Cards { get { return cards; } }
        public int TotalValue { get { return totalValue; } }

        #endregion

        #region Private Members

        private List<Card> cards;
        private int totalValue;
        private bool hasSoftAce; // a soft ace is an ace that has a value of 11

        #endregion

        #region Constructor

        public Hand()
        {
            cards = new List<Card>();
            totalValue = 0;
            hasSoftAce = false;
        }

        #endregion

        #region Public Functions

        public void AddCard(Card card)
        {
            cards.Add(card);

            if (card.Name == CardName.ACE)
            {
                hasSoftAce = true;
            }

            totalValue += card.Value;

            // if the total value is over 21 and has a soft ace (11), convert soft ace to 1
            if (totalValue > 21 && hasSoftAce)
            {
                totalValue -= 10;
                Card softAce = cards.Where(c => c.Name == CardName.ACE && c.Value == 11).FirstOrDefault();
                if (softAce != null)
                {
                    softAce.ToggleAceValue();
                    //hasSoftAce = false;
                }
            }
        }

        public Hand Clone()
        {
            return new Hand
            {
                cards = new List<Card>(this.cards),
                totalValue = this.totalValue,
                hasSoftAce = this.hasSoftAce
            };
        }

        #endregion



    }
}
