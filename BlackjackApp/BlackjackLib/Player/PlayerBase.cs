using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{

    public class PlayerBase
    {
        public List<Hand> hands;
        public int currentHandIndex;

        public Hand CurrentHand
        {
            get
            {
                if (hands == null || currentHandIndex > (hands.Count - 1)) return null;
                return hands[currentHandIndex];
            }
        }

        public PlayerBase()
        {
            hands = new List<Hand> { new Hand() };
            currentHandIndex = 0;
        }

        public void AddCard(Card card)
        {
            hands[currentHandIndex].AddCard(card);
        }

        public void Split()
        {
            hands.Add(new Hand());
        }
    }


}
