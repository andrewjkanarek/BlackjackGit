using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public abstract class ProbCounterBase
    {
        #region Private Members

        private Stats beforeHitStats;
        private Stats afterHitStats;

        // key: outcome, probability of outcome
        private Dictionary<PossibleOutcome, decimal> playerProbDict = new Dictionary<PossibleOutcome, decimal>
        {
            { PossibleOutcome.SIXTEEN, 0 },
            { PossibleOutcome.SEVENTEEN, 0 },
            { PossibleOutcome.EIGHTEEN, 0 },
            { PossibleOutcome.NINETEEN, 0 },
            { PossibleOutcome.TWENTY, 0 },
            { PossibleOutcome.TWENTYONE, 0 },
            { PossibleOutcome.NOBUST, 0 }
        };

        // key: outcome, probability of outcome
        private Dictionary<PossibleOutcome, decimal> dealerProbDict = new Dictionary<PossibleOutcome, decimal>
        {
            { PossibleOutcome.SIXTEEN, 0 },
            { PossibleOutcome.SEVENTEEN, 0 },
            { PossibleOutcome.EIGHTEEN, 0 },
            { PossibleOutcome.NINETEEN, 0 },
            { PossibleOutcome.TWENTY, 0 },
            { PossibleOutcome.TWENTYONE, 0 },
            { PossibleOutcome.NOBUST, 0 }
        };

        #endregion

        #region Public Members

        public Stats BeforeHitStats { get { return beforeHitStats; } }
        public Stats AfterHitStats { get { return afterHitStats; } }

        #endregion

        protected ProbCounterBase()
        {
            beforeHitStats = new Stats();
            afterHitStats = new Stats();
        }

        #region Public Functions

        public abstract void UpdateStats(Hand playerHand, Hand dealerHand, Deck deck);

        #endregion

        #region Private Functions

        protected void ResetStats()
        {
            // reset prob dicts
            ResetProbDict(playerProbDict);
            ResetProbDict(dealerProbDict);
            return;
        }

        protected void SetDealerProbMap(Hand dealerHand, Deck deck, decimal probability)
        {
            // if hand total is over 21, the dealer busts
            if (dealerHand.TotalValue > 21)
            {
                dealerProbDict[PossibleOutcome.BUST] += probability;
                return;
            }

            // if the hand is over 16, the dealer cannot hit
            if (dealerHand.TotalValue >= 16 && dealerHand.TotalValue <= 21)
            {
                PossibleOutcome outcome = deck.PossibleOutcomeValues[dealerHand.TotalValue];
                dealerProbDict[outcome] += probability;
                return;
            }

            // if the hand is below 16, dealer must hit
            // we need to iterate possible values
            foreach (var keyvalue in deck.CardCountDict)
            {
                // if there are none of this card left, it cannot appear
                if (keyvalue.Value <= 0) continue;

                // the probability of getting a card AT THIS POINT,
                // is the normal prob of selecting the card, multiplied by probability of getting to this point
                decimal cardProb = deck.GetCardProb(keyvalue.Key) * probability;

                // update temporary dealer hand and deck
                Card card = new Card(keyvalue.Key);
                dealerHand.AddCard(card);
                deck.RemoveCard(keyvalue.Key);

                // update probability with new card and new probability
                SetDealerProbMap(dealerHand, deck, cardProb);
            }
        }

        protected void SetPlayerProbMap(Hand playerHand, Deck deck)
        {

        }

        #endregion

        #region Helpers

        private void ResetProbDict(Dictionary<PossibleOutcome, decimal> probDict)
        {
            if (probDict == null || probDict.Keys.Count == 0) return;

            foreach (var key in probDict.Keys)
            {
                probDict[key] = 0;
            }
        }

        #endregion

    }
}
