using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class ProbCounterSlow : ProbCounterBase
    {
        public ProbCounterSlow() : base() { }
       

        protected override void SetDealerProbMap(Hand dealerHand, Deck deck, decimal probability)
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
        protected override void SetPlayerProbMap(Hand playerHand, Deck deck, decimal probability)
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

    }
}
