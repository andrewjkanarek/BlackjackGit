using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackLib
{
    public abstract class ProbCounterBase
    {
        #region Private Members

        private Stats beforeHitStats;
        private Stats afterHitStats;

        // key: outcome, probability of outcomes AFTER player hits
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

        // key: outcome, probability of outcome of dealer hand
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
            if (dealerHand.TotalValue >= 16)
            {
                PossibleOutcome outcome = deck.PossibleOutcomeValues[dealerHand.TotalValue];
                dealerProbDict[outcome] += probability;
                return;
            }

            // if the hand is below 16, dealer must hit
            // we need to iterate possible values
            foreach (CardName cardName in deck.CardCountDict.Keys.ToList())
            {

                int remainingCardCount = deck.CardCountDict[cardName];

                // if there are none of this card left, it cannot appear
                if (remainingCardCount <= 0) continue;

                // the probability of getting a card AT THIS POINT,
                // is the normal prob of selecting the card, multiplied by probability of getting to this point
                decimal cardProb = deck.GetCardProb(cardName) * probability;

                // update temporary dealer hand and deck
                Card card = new Card(cardName);
                dealerHand.AddCard(card);
                deck.RemoveCard(cardName);

                // update probability with new card and new probability
                SetDealerProbMap(dealerHand, deck, cardProb);
            }
        }

        // the probability a player wins is:
        //  1) if the dealer busts AND they don't
        //  2) if the dealer has a higher hand than the dealer
        //      - the dealer must have at least a 16, so we need to calculate prob of each value between 16 and 21
        protected void SetPlayerProbMap(Hand playerHand, Deck deck, decimal probability)
        {

            if (playerHand.TotalValue >= 21) return;

            // calculate all possibilities if player hits
            foreach (CardName cardName in deck.CardCountDict.Keys.ToList())
            {
                int remainingCardCount = deck.CardCountDict[cardName];

                // if there are none of this card left, it cannot appear
                if (remainingCardCount <= 0) continue;

                // the probability of getting a card AT THIS POINT,
                // is the normal prob of selecting the card, multiplied by probability of getting to this point
                decimal cardProb = deck.GetCardProb(cardName) * probability;

                // update temporary dealer hand and deck
                Card card = new Card(cardName);
                playerHand.AddCard(card);
                deck.RemoveCard(cardName);

                if (playerHand.TotalValue > 21) continue;

                if (playerHand.TotalValue >= 16)
                {
                    PossibleOutcome outcome = deck.PossibleOutcomeValues[playerHand.TotalValue];
                    playerProbDict[outcome] += probability;
                }

                // update probability with new card and new probability
                SetPlayerProbMap(playerHand, deck, cardProb);
            }
        }

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
