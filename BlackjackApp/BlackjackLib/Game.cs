using System;

namespace BlackjackLib
{
    public class Game
    {
        private Deck deck;
        private Player player;
        private Dealer dealer;
        private ProbCounterBase probCounter;
        private GameSettings gameSettings;

        public Deck Deck { get { return deck; } }
        public Player Player { get { return player; } }
        public Dealer Dealer { get { return dealer; } }
        public ProbCounterBase ProbCounter { get { return probCounter; } }
        public GameSettings GameSettings { get { return gameSettings; } }


        public Game(GameSettings settings)
        {
            gameSettings = settings;
            deck = new Deck(gameSettings.NumDecks);
            player = new Player();
            dealer = new Dealer();
            probCounter = new ProbCounterFast();
        }

        public delegate void AddCardDelegate(Card card);
        public void AddCard(CardName cardName, PlayerBase player)
        {
            if (cardName == CardName.NONE) return;

            Card card = new Card(cardName);
            player.AddCard(card);
            deck.RemoveCard(cardName);
        }

        public void UpdateStats()
        {
            probCounter.UpdateStats(player.CurrentHand, dealer.CurrentHand, deck);
        }
    }
}
