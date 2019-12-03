using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackLib
{
    public abstract class ProbCounterBase
    {
        #region Public Members

        public Stats BeforeHitStats { get { return beforeHitStats; } }
        public Stats AfterHitStats { get { return afterHitStats; } }
        public Decision Decision { get { return decision; } }

        #endregion

        #region Private Members

        protected Stats beforeHitStats;
        protected Stats afterHitStats;
        protected Decision decision;

        // key: outcome, probability of outcomes AFTER player hits
        protected Dictionary<PossibleOutcome, decimal> playerProbDict = new Dictionary<PossibleOutcome, decimal>
        {
            { PossibleOutcome.SEVENTEEN, 0 },
            { PossibleOutcome.EIGHTEEN, 0 },
            { PossibleOutcome.NINETEEN, 0 },
            { PossibleOutcome.TWENTY, 0 },
            { PossibleOutcome.TWENTYONE, 0 },
            { PossibleOutcome.NOBUST, 0 }
        };

        // key: outcome, probability of outcome of dealer hand
        protected Dictionary<PossibleOutcome, decimal> dealerProbDict = new Dictionary<PossibleOutcome, decimal>
        {
            { PossibleOutcome.SEVENTEEN, 0 },
            { PossibleOutcome.EIGHTEEN, 0 },
            { PossibleOutcome.NINETEEN, 0 },
            { PossibleOutcome.TWENTY, 0 },
            { PossibleOutcome.TWENTYONE, 0 },
            { PossibleOutcome.BUST, 0 }
        };

        #endregion

        #region Constructor

        protected ProbCounterBase()
        {
            beforeHitStats = new Stats();
            afterHitStats = new Stats();
        }

        #endregion

        #region Public Functions

        public void UpdateStats(Hand playerHand, Hand dealerHand, Deck deck)
        {
            ResetStats();

            SetDealerProbMap(dealerHand, deck, 1);
            SetPlayerProbMap(playerHand, deck, 1);

            beforeHitStats = GetBeforeHitStats(playerHand, dealerHand);
            afterHitStats = GetAfterHitStats();

            decision = GetDecision();
        }

        #endregion

        #region Private Functions

        #region Set / Reset Dictionaries

        protected void ResetStats()
        {
            // reset prob dicts
            ResetProbDict(playerProbDict);
            ResetProbDict(dealerProbDict);
            return;
        }

        protected abstract void SetDealerProbMap(Hand dealerHand, Deck deck, decimal probability);

        protected abstract void SetPlayerProbMap(Hand playerHand, Deck deck, decimal probability);

        #endregion

        #region Get Prob Stats

        protected Stats GetBeforeHitStats(Hand playerHand, Hand dealerHand)
        {
            return new Stats
            {
                win = GetProbWinBeforeHit(playerHand),
                lose = GetProbLoseBeforeHit(playerHand, dealerHand),
                push = GetProbPushBeforeHit(playerHand)
            };
        }

        protected Stats GetAfterHitStats()
        {
            Stats s = new Stats();
            s.win = GetProbWinAfterHit();
            s.push = GetProbPushAfterHit();
            s.lose = 1 - s.win - s.push;
            return s;
        }

        #region Before Hitting

        protected decimal GetProbWinBeforeHit(Hand playerHand)
        {

            if (playerHand.TotalValue < 17)
            {
                return dealerProbDict[PossibleOutcome.BUST];
            }

            if (playerHand.TotalValue > 21)
            {
                return (decimal)0;
            }

            return GetProbHigherHand(playerHand, dealerProbDict);

        }

        protected decimal GetProbLoseBeforeHit(Hand playerHand, Hand dealerHand)
        {
            // if the dealer doesn't bust, and player has les than 17, dealer wins
            if (playerHand.TotalValue < 17)
            {
                return 1 - dealerProbDict[PossibleOutcome.BUST];
            }

            return GetProbLowerHand(playerHand, dealerProbDict);
        }

        protected decimal GetProbPushBeforeHit(Hand playerHand)
        {
            if (playerHand.TotalValue > 21 || playerHand.TotalValue < 17) return (decimal)0;

            PossibleOutcome outcome = Deck.PossibleOutcomeValues[playerHand.TotalValue];

            return dealerProbDict[outcome];

        }


        protected decimal GetProbHigherHand(Hand hand, Dictionary<PossibleOutcome, decimal> probDict)
        {
            if (hand.TotalValue == 18)
            {
                return probDict[PossibleOutcome.SEVENTEEN];
            }
            else if (hand.TotalValue == 19)
            {
                return probDict[PossibleOutcome.SEVENTEEN] + probDict[PossibleOutcome.EIGHTEEN];
            }
            else if (hand.TotalValue == 20)
            {
                return probDict[PossibleOutcome.SEVENTEEN] + probDict[PossibleOutcome.EIGHTEEN] + probDict[PossibleOutcome.NINETEEN];
            }
            else if (hand.TotalValue == 21)
            {
                return probDict[PossibleOutcome.SEVENTEEN] + probDict[PossibleOutcome.EIGHTEEN] +
                       probDict[PossibleOutcome.NINETEEN] + probDict[PossibleOutcome.TWENTY];
            }

            return 0;
        }

        protected decimal GetProbLowerHand(Hand hand, Dictionary<PossibleOutcome, decimal> probDict)
        {
            if (hand.TotalValue == 20)
            {
                return probDict[PossibleOutcome.TWENTYONE];
            }
            else if (hand.TotalValue == 19)
            {
                return probDict[PossibleOutcome.TWENTY] + probDict[PossibleOutcome.TWENTYONE];
            }
            else if (hand.TotalValue == 18)
            {
                return probDict[PossibleOutcome.NINETEEN] + probDict[PossibleOutcome.TWENTY] + probDict[PossibleOutcome.TWENTYONE];
            }
            else if (hand.TotalValue == 17)
            {
                return probDict[PossibleOutcome.EIGHTEEN] + probDict[PossibleOutcome.NINETEEN] +
                       probDict[PossibleOutcome.TWENTY] + probDict[PossibleOutcome.TWENTYONE];
            }

            return 0;
        }

        #endregion

        #region After Hitting

        protected decimal GetProbWinAfterHit()
        {
            // two possibilities of winning

            // dealer busts and player doesn't
            decimal probWinOnDealerBust = playerProbDict[PossibleOutcome.NOBUST] * dealerProbDict[PossibleOutcome.BUST];

            // neither bust but player has better hand
            decimal probPlayerHigherHand = GetProbHigherHandAfterHit(playerProbDict, dealerProbDict);

            return probWinOnDealerBust + probPlayerHigherHand;
        }

        // the probability of dealer winning:
        //		a) player busts
        //		OR
        //		b) dealer>player and dealer doesn't bust
        protected decimal GetProbLoseAfterHit()
        {
            decimal probPlayerBust = 1 - playerProbDict[PossibleOutcome.NOBUST];
            decimal probDealerHigherHand = GetProbHigherHandAfterHit(dealerProbDict, playerProbDict);

            return probPlayerBust + probDealerHigherHand;
        }

        protected decimal GetProbPushAfterHit()
        {
            decimal result = 0;

            result += (dealerProbDict[PossibleOutcome.SEVENTEEN] * playerProbDict[PossibleOutcome.SEVENTEEN]);
            result += (dealerProbDict[PossibleOutcome.EIGHTEEN] * playerProbDict[PossibleOutcome.EIGHTEEN]);
            result += (dealerProbDict[PossibleOutcome.NINETEEN] * playerProbDict[PossibleOutcome.NINETEEN]);
            result += (dealerProbDict[PossibleOutcome.TWENTY] * playerProbDict[PossibleOutcome.TWENTY]);
            result += (dealerProbDict[PossibleOutcome.TWENTYONE] * playerProbDict[PossibleOutcome.TWENTYONE]);

            return result;
        }

        // get probability prob Dict 1 is higher than prob Dict 2
        protected decimal GetProbHigherHandAfterHit(Dictionary<PossibleOutcome, decimal> probDict1,
            Dictionary<PossibleOutcome, decimal> probDict2)
        {
            decimal result = 0;

            // dealer gets 17 and player gets higher
            result += (probDict2[PossibleOutcome.SEVENTEEN] *
                (probDict1[PossibleOutcome.EIGHTEEN] + probDict1[PossibleOutcome.NINETEEN] +
                 probDict1[PossibleOutcome.TWENTY] + probDict1[PossibleOutcome.TWENTYONE]));

            // dealer gets 18 and player gets higher
            result += (probDict2[PossibleOutcome.EIGHTEEN] *
                (probDict1[PossibleOutcome.NINETEEN] + probDict1[PossibleOutcome.TWENTY] +
                 probDict1[PossibleOutcome.TWENTYONE]));

            // dealer gets 19 and player gets higher
            result += (probDict2[PossibleOutcome.NINETEEN] *
                (probDict1[PossibleOutcome.TWENTY] + probDict1[PossibleOutcome.TWENTYONE]));

            // dealer gets 20 and player gets higher
            result += (probDict2[PossibleOutcome.TWENTY] * probDict1[PossibleOutcome.TWENTYONE]);

            return result;
        }

        protected Decision GetDecision()
        {
            return Decision.HIT;
        }

        #endregion




        #endregion

        #endregion

        #region Helpers

        private void ResetProbDict(Dictionary<PossibleOutcome, decimal> probDict)
        {
            if (probDict == null || probDict.Keys.Count == 0) return;

            foreach (var key in probDict.Keys.ToList())
            {
                probDict[key] = 0;
            }
        }

        #endregion

    }
}
