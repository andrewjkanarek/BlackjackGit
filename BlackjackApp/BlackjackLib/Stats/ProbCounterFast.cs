using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class ProbCounterFast : ProbCounterBase
    {
        public ProbCounterFast() : base() { }

        public override void UpdateStats(Hand playerHand, Hand dealerHand, Deck deck)
        {
            return;
        }
    }
}
