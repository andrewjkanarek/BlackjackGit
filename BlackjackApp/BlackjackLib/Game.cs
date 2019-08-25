using System;

namespace BlackjackLib
{
    public class Game
    {
        private Deck deck;
        private Player player;
        private Dealer dealer;
        private ProbCounterSlow probCounter;

        public Deck Deck { get { return deck; } }
        public Player Player { get { return player; } }
        public Dealer Dealer { get { return dealer; } }
        public ProbCounterSlow ProbCounter { get { return probCounter;  } }


        public Game(GameSettings settings)
        {
            deck = new Deck(settings.NumDecks);
            player = new Player();
            dealer = new Dealer();
            probCounter = new ProbCounterSlow();
        }

        public void AddPlayerCard(CardName cardName)
        {
            player.AddCard(deck.CardDict[cardName]);
            deck.RemoveCard(cardName);
            //probCounter.UpdateStats();
        }

        public void AddDealerCard(CardName cardName)
        {
            dealer.AddCard(deck.CardDict[cardName]);
            deck.RemoveCard(cardName);
            //probCounter.UpdateStats(player.CurrentHand, dealer.CurrentHand, deck);
        }
    }
}
