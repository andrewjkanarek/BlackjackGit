using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{


    public abstract class PlayerBase
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
            currentHandIndex = 0;
        }

        public void AddCard(Card card)
        {
            CurrentHand.AddCard(card);
        }

        public abstract void Split();
    }


}
