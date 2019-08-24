using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Deck
    {
        public Dictionary<CardName, int> cardCountDict;
        public Dictionary<CardName, Card> cardDict;


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

            cardDict = new Dictionary<CardName, Card>
            {
                { CardName.TWO, new Card(CardName.TWO, 2) },
                { CardName.THREE, new Card(CardName.THREE, 3) },
                { CardName.FOUR, new Card(CardName.FOUR, 4) },
                { CardName.FIVE, new Card(CardName.FIVE, 5) },
                { CardName.SIX, new Card(CardName.SIX, 6) },
                { CardName.SEVEN, new Card(CardName.SEVEN, 7) },
                { CardName.EIGHT, new Card(CardName.EIGHT, 8) },
                { CardName.NINE, new Card(CardName.NINE, 9) },
                { CardName.TEN, new Card(CardName.TEN, 10) },
                { CardName.ACE, new Card(CardName.ACE, 11) }
            };


        }
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
}
