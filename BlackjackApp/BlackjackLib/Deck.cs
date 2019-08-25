using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Deck
    {
        #region Private Members

        private Dictionary<CardName, int> cardCountDict;
        private Dictionary<int, PossibleOutcome> possibleOutcomeValues;
        private int totalCardCount;

        #endregion

        #region Public Members

        public Dictionary<CardName, int> CardCountDict { get { return cardCountDict; } }
        public Dictionary<int, PossibleOutcome> PossibleOutcomeValues { get { return possibleOutcomeValues; } }
        public int TotalCardCount { get { return totalCardCount; } }

        public static Dictionary<CardName, int> CardValDict = new Dictionary<CardName, int>
        {
            { CardName.TWO, 2 },
            { CardName.THREE, 3 },
            { CardName.FOUR, 4 },
            { CardName.FIVE, 5 },
            { CardName.SIX, 6 },
            { CardName.SEVEN, 7 },
            { CardName.EIGHT, 8 },
            { CardName.NINE, 9 },
            { CardName.TEN, 10 },
            { CardName.ACE, 11 }

        };

        #endregion


        #region Constructor

        public Deck(int numDecks)
        {
            cardCountDict = new Dictionary<CardName, int>
            {
                { CardName.TWO, 4 * numDecks },
                { CardName.THREE, 4 * numDecks },
                { CardName.FOUR, 4 * numDecks },
                { CardName.FIVE, 4 * numDecks },
                { CardName.SIX, 4 * numDecks },
                { CardName.SEVEN, 4 * numDecks },
                { CardName.EIGHT, 4 * numDecks },
                { CardName.NINE, 4 * numDecks },
                { CardName.TEN, 16 * numDecks },
                { CardName.ACE, 4 * numDecks }

            };

            possibleOutcomeValues = new Dictionary<int, PossibleOutcome>
            {
                { 16, PossibleOutcome.SIXTEEN },
                { 17, PossibleOutcome.SEVENTEEN },
                { 18, PossibleOutcome.EIGHTEEN },
                { 19, PossibleOutcome.NINETEEN },
                { 20, PossibleOutcome.TWENTY },
                { 21, PossibleOutcome.TWENTYONE }
            };

            totalCardCount = 52 * numDecks;


        }

        #endregion

        #region Public Functions

        public decimal GetCardProb(CardName cardName)
        {
            return (decimal)cardCountDict[cardName] / totalCardCount;
        }

        public void RemoveCard(CardName cardName)
        {
            if (cardCountDict[cardName] <= 0) return;

            cardCountDict[cardName]--;
            totalCardCount--;
        }

        public void AddCard(CardName cardName)
        {
            cardCountDict[cardName]++;
            totalCardCount++;
        }

        #endregion
    }

    public enum CardName
    {
        NONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        ACE
    }

    public enum PossibleOutcome
    {
        SIXTEEN,
        SEVENTEEN,
        EIGHTEEN,
        NINETEEN,
        TWENTY,
        TWENTYONE,
        BUST,
        NOBUST
    }
}
