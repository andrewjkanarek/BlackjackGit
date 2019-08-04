using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Deck
    {
        public static Dictionary<CardName, int> cardCountDict;
        public static Dictionary<CardName, Card> cardDict;


        public Deck(int numDecks)
        {
            cardCountDict = new Dictionary<CardName, int>
            {
                { CardName.TWO, 4 * numDecks }
            };

            cardDict = new Dictionary<CardName, Card>
            {
                { CardName.TWO, new Card(CardName.TWO, 2) }
            };


        }
    }

    public enum CardName
    {
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
