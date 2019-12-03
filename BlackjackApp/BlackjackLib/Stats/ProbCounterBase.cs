using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackLib
{
    public abstract class ProbCounterBase
    {
        #region Private Members

        public Stats beforeHitStats;
        public Stats afterHitStats;
        public Decision decision;

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

        public abstract void UpdateStats(Hand playerHand, Hand dealerHand, Deck deck);

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


        //protected void SetDealerProbMap(Hand dealerHand, Deck deck, decimal probability)
        //{
        //    if (probability == 0) return;

        //    // if hand total is over 21, the dealer busts
        //    if (dealerHand.TotalValue > 21)
        //    {
        //        dealerProbDict[PossibleOutcome.BUST] += probability;
        //        return;
        //    }

        //    // if the hand is over 16, the dealer cannot hit
        //    // the probability of getting 19 is NOT prob(17) + prob(2)
        //    if (dealerHand.TotalValue >= 17)
        //    {
        //        PossibleOutcome outcome = Deck.PossibleOutcomeValues[dealerHand.TotalValue];
        //        dealerProbDict[outcome] += probability;
        //        return;
        //    }

        //    // if the hand is below 16, dealer must hit
        //    // we need to iterate possible values
        //    foreach (CardName cardName in deck.GetRemainingCards())
        //    {

        //        int remainingCardCount = deck.CardCountDict[cardName];

        //        // if there are none of this card left, it cannot appear
        //        if (remainingCardCount <= 0) continue;

        //        // the probability of getting a card AT THIS POINT,
        //        // is the normal prob of selecting the card, multiplied by probability of getting to this point
        //        decimal cardProb = deck.GetCardProb(cardName) * probability;

        //        // update temporary dealer hand and deck
        //        Card card = new Card(cardName);
        //        Hand handCopy = dealerHand.Clone();
        //        Deck deckCopy = deck.Clone();
        //        handCopy.AddCard(card);
        //        deckCopy.RemoveCard(cardName);

        //        // update probability with new card and new probability
        //        SetDealerProbMap(handCopy, deckCopy, cardProb);

        //        // break loop if hand is already over 16
        //        if (dealerHand.TotalValue >= 17) break;
        //    }
        //}

        protected void SetDealerProbMap(Hand dealerHand, Deck deck, decimal probability)
        {
            // if hand total is over 21, the dealer busts
            if (dealerHand.TotalValue > 21)
            {
                dealerProbDict[PossibleOutcome.BUST] += probability;
                return;
            }

            // if the dealer has a 17 (not COULD have), then they cannot get anything else (must stick)
            if (dealerHand.TotalValue >= 17 && probability == 1)
            {
                PossibleOutcome outcome = Deck.PossibleOutcomeValues[dealerHand.TotalValue];
                dealerProbDict[outcome] += probability;
                return;
            }

            // calculate all possibilities if player hits
            foreach (CardName cardName in deck.GetRemainingCards())
            {
                // if there are none of this card left, it cannot appear
                int remainingCardCount = deck.CardCountDict[cardName];
                if (remainingCardCount <= 0) continue;

                // update temporary dealer hand and deck
                Card card = new Card(cardName);
                // the probability of getting a card AT THIS POINT,
                // is the normal prob of selecting the card, multiplied by probability of getting to this point
                decimal cardProb = deck.GetCardProb(cardName) * probability;

                // add to new hand
                Hand handCopy = dealerHand.Clone();
                handCopy.AddCard(card);
                // remove from new deck
                Deck deckCopy = deck.Clone();
                deckCopy.RemoveCard(cardName);

                if (handCopy.TotalValue > 21)
                {
                    dealerProbDict[PossibleOutcome.BUST] += cardProb;
                }
                else if (handCopy.TotalValue >= 17)
                {
                    PossibleOutcome outcome = Deck.PossibleOutcomeValues[handCopy.TotalValue];
                    dealerProbDict[outcome] += cardProb;
                    continue;
                }

                // update probability with new card and new probability
                SetDealerProbMap(handCopy, deckCopy, cardProb);
            }
        }

        // the probability a player wins is:
        //  1) if the dealer busts AND the player doesn't
        //  2) if the dealer has a higher hand than the dealer
        //      - the dealer must have at least a 16, so we need to calculate prob of each value between 16 and 21
        protected void SetPlayerProbMap(Hand playerHand, Deck deck, decimal probability)
        {

            if (playerHand.TotalValue >= 21) return;

            // calculate all possibilities if player hits
            foreach (CardName cardName in deck.GetRemainingCards())
            {
                int remainingCardCount = deck.CardCountDict[cardName];

                // if there are none of this card left, it cannot appear
                if (remainingCardCount <= 0) continue;

                // update temporary dealer hand and deck
                Card card = new Card(cardName);

                // add to new hand
                Hand handCopy = playerHand.Clone();
                handCopy.AddCard(card);

                if (handCopy.TotalValue > 21) continue;

                // remove from new deck
                Deck deckCopy = deck.Clone();              
                deckCopy.RemoveCard(cardName);

                // the probability of getting a card AT THIS POINT,
                // is the normal prob of selecting the card, multiplied by probability of getting to this point
                decimal cardProb = deck.GetCardProb(cardName) * probability;

                playerProbDict[PossibleOutcome.NOBUST] += cardProb;

                if (handCopy.TotalValue >= 17)
                {
                    PossibleOutcome outcome = Deck.PossibleOutcomeValues[handCopy.TotalValue];
                    playerProbDict[outcome] += cardProb;
                }

                // update probability with new card and new probability
                SetPlayerProbMap(handCopy, deckCopy, cardProb);
            }
        }

        #endregion

        #region Get Prob Stats

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
