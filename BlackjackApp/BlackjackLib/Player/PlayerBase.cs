using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{

    public abstract class PlayerBase
    {
        public List<HandBase> hands;
        public int currentHandIndex;

        public HandBase CurrentHand { get { return hands[currentHandIndex]; } }

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
