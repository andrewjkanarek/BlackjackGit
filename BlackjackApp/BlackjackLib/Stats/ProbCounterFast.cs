using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackLib
{
    public class ProbCounterFast : ProbCounterBase
    {
        private Dictionary<int, decimal> probDict;

        public ProbCounterFast() : base()
        {
            probDict = new Dictionary<int, decimal>();
        }

        protected override void SetProbMap(Deck deck)
        {
            probDict[0] = 0;
            probDict[1] = deck.GetCardProb(CardName.ACE);
            probDict[2] = deck.GetCardProb(CardName.TWO) + (probDict[1] * probDict[1]);
            for (int i = 3; i <= 21; ++i)
            {
                // if there is a card with the given value, the probability of getting that value is AT LEAST
                // the probability of getting that card
                CardName ?cardName = Deck.GetCardName(i);
                probDict[i] = (cardName == null) ? 0 : deck.GetCardProb((CardName)cardName);

                // find all combinations of numbers that add up to i
                List<List<int>> combinations = FindCombinations(i);

                foreach (List<int> combination in combinations)
                {
                    // we want to exclude the scenario of just getting the number since that is
                    // the probability being calculated
                    if (combination.Count == 1) continue;

                    decimal prob = 1;
                    foreach (int num in combination)
                    {
                        prob *= probDict[num];
                    }

                    probDict[i] += prob;
                }
            }
        }

        protected override void SetDealerProbMap(Hand dealerHand, Deck deck, decimal probability)
        {
            // if hand total is over 21, the dealer busts
            if (dealerHand.TotalValue > 21)
            {
                dealerProbDict[PossibleOutcome.BUST] += probability;
                return;
            }

            // if the dealer has a 17, then they cannot get anything else (must stick)
            if (dealerHand.TotalValue >= 17)
            {
                PossibleOutcome outcome = Deck.PossibleOutcomeValues[dealerHand.TotalValue];
                dealerProbDict[outcome] += probability;
                return;
            }

            // calculate the probabilty of getting each value 17-21
            List<int> values = new List<int> { 17, 18, 19, 20, 21 };
            foreach (int i in values)
            {
                PossibleOutcome outcome = Deck.PossibleOutcomeValues[i];
                dealerProbDict[outcome] = probDict[i - dealerHand.TotalValue];
            }

            // calculate the probability the dealer busts
            foreach (int val in probDict.Keys.ToList())
            {
                if (dealerHand.TotalValue + val <= 21) continue;

                dealerProbDict[PossibleOutcome.BUST] += probDict[val];
            }
        }

        // the probability a player wins is:
        //  1) if the dealer busts AND the player doesn't
        //  2) if the dealer has a higher hand than the dealer
        //      - the dealer must have at least a 16, so we need to calculate prob of each value between 16 and 21
        protected override void SetPlayerProbMap(Hand playerHand, Deck deck, decimal probability)
        {
            // calculate the probability the player hits and gets less than 17
            foreach (int val in probDict.Keys.ToList())
            {
                if (playerHand.TotalValue + val >= 17) continue;

                playerProbDict[PossibleOutcome.NOBUST] += probDict[val];
            }

            // calculate the probabilty of getting each value 17-21
            List<int> values = new List<int> { 17, 18, 19, 20, 21 };
            foreach (int i in values)
            {
                PossibleOutcome outcome = Deck.PossibleOutcomeValues[i];
                playerProbDict[outcome] = (playerHand.TotalValue < i) ? probDict[i - playerHand.TotalValue] : 0;
            }
        }

        /* arr - array to store the  
        combination  
        index - next location in array  
        num - given number  
        reducedNum - reduced number */
        static void FindCombinationsUtil(ref List<List<int>> result, int[] arr, int index,
                                         int num, int reducedNum)
        {
            // Base condition  
            if (reducedNum < 0)
                return;

            // If combination is  
            // found, print it  
            if (reducedNum == 0)
            {
                List<int> l = new List<int>();
                for (int i = 0; i < index; i++)
                    l.Add(arr[i]);
                result.Add(l);
                return;
            }

            // Find the previous number  
            // stored in arr[]. It helps  
            // in maintaining increasing  
            // order  
            int prev = (index == 0) ?
                                  1 : arr[index - 1];

            // note loop starts from  
            // previous number i.e. at  
            // array location index - 1  
            for (int k = prev; k <= num; k++)
            {
                // next element of  
                // array is k  
                arr[index] = k;

                // call recursively with  
                // reduced number  
                FindCombinationsUtil(ref result, arr, index + 1, num,
                                         reducedNum - k);
            }
        }

        /* Function to find out all  
        combinations of positive  
        numbers that add upto given  
        number. It uses FindCombinationsUtil() */
        static List<List<int>> FindCombinations(int n)
        {
            List<List<int>> result = new List<List<int>>();

            // array to store the combinations  
            // It can contain max n elements  
            int[] arr = new int[n];

            // find all combinations  
            FindCombinationsUtil(ref result, arr, 0, n, n);

            return result;
        }
    }
}
