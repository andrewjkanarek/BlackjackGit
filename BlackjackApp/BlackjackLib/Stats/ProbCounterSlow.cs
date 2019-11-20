using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class ProbCounterSlow : ProbCounterBase
    {
        public ProbCounterSlow() : base() { }
        
        public override void UpdateStats(Hand playerHand, Hand dealerHand, Deck deck)
        {
            ResetStats();

            SetDealerProbMap(dealerHand, deck, 1);
            SetPlayerProbMap(playerHand, deck, 1);

            beforeHitStats.win = GetProbWinBeforeHit(playerHand);
            beforeHitStats.lose = GetProbLoseBeforeHit(playerHand, dealerHand);
            beforeHitStats.push = GetProbPushBeforeHit(playerHand);

            afterHitStats.win = GetProbWinAfterHit();
            afterHitStats.push = GetProbPushAfterHit();
            afterHitStats.lose = 1 - afterHitStats.win - afterHitStats.push;

            decision = GetDecision();
        }

    }
}
