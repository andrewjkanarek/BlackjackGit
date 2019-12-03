using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class ProbCounterFast : ProbCounterBase
    {
        public ProbCounterFast() : base() { }

        protected override void SetDealerProbMap(Hand dealerHand, Deck deck, decimal probability)
        {
            return;
        }

        // the probability a player wins is:
        //  1) if the dealer busts AND the player doesn't
        //  2) if the dealer has a higher hand than the dealer
        //      - the dealer must have at least a 16, so we need to calculate prob of each value between 16 and 21
        protected override void SetPlayerProbMap(Hand playerHand, Deck deck, decimal probability)
        {

            return;
        }
    }
}
