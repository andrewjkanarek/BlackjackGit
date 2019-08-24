using System;

namespace BlackjackLib
{
    public class Game
    {
        public Deck deck;
        public Player player;
        public Dealer dealer;


        public Game(GameSettings settings)
        {
            deck = new Deck(settings.NumDecks);
            player = new Player();
            dealer = new Dealer();
        }

        public void AddPlayerCard(CardName cardName)
        {
            player.AddCard(deck.cardDict[cardName]);
        }

        public void AddDealerCard(CardName cardName)
        {
            dealer.AddCard(deck.cardDict[cardName]);
        }
    }
}
