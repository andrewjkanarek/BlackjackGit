using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackLib
{
    public class Deck
    {
        #region Private Members

        private Dictionary<CardName, int> cardCountDict;
        private int totalCardCount;
        private int deckCount;

        #endregion

        #region Public Members

        public const int MAX_DECK_COUNT = 8;

        public int DeckCount {  get { return deckCount; } }

        public Dictionary<CardName, int> CardCountDict { get { return cardCountDict; } }

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

        public static Dictionary<int, PossibleOutcome> PossibleOutcomeValues = new Dictionary<int, PossibleOutcome>
        {
            { 17, PossibleOutcome.SEVENTEEN },
            { 18, PossibleOutcome.EIGHTEEN },
            { 19, PossibleOutcome.NINETEEN },
            { 20, PossibleOutcome.TWENTY },
            { 21, PossibleOutcome.TWENTYONE }
        };

        #endregion


        #region Constructor

        public Deck() : this(4)
        {
            deckCount = 4;
        }

        public Deck(int numDecks)
        {
            deckCount = numDecks;

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

            totalCardCount = 52 * numDecks;


        }

        #endregion

        #region Public Functions

        public Deck Clone()
        {
            return new Deck
            {
                cardCountDict = new Dictionary<CardName, int>(this.cardCountDict),
                totalCardCount = this.totalCardCount
            };
        }

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

        public void UpdateDeckCount(int newNumDecks)
        {
            if (newNumDecks < 0 || newNumDecks > MAX_DECK_COUNT) return;

            int deckCountDiff = newNumDecks - deckCount;

            foreach (CardName cardName in cardCountDict.Keys.ToList())
            {
                int cardsPerDeck = 4;
                if (cardName == CardName.TEN)
                {
                    cardsPerDeck = 16;
                    
                }

                cardCountDict[cardName] = Math.Max(0, cardCountDict[cardName] + (cardsPerDeck * deckCountDiff));

            }

            deckCount = newNumDecks;
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
