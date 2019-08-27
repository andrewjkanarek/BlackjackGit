﻿using System;

namespace BlackjackLib
{
    public class Game
    {
        private Deck deck;
        private Player player;
        private Dealer dealer;
        private ProbCounterSlow probCounter;
        private GameSettings gameSettings;

        public Deck Deck { get { return deck; } }
        public Player Player { get { return player; } }
        public Dealer Dealer { get { return dealer; } }
        public ProbCounterSlow ProbCounter { get { return probCounter;  } }
        public GameSettings GameSettings { get { return gameSettings; } }


        public Game(GameSettings settings)
        {
            gameSettings = settings;
            deck = new Deck(gameSettings.NumDecks);
            player = new Player();
            dealer = new Dealer();
            probCounter = new ProbCounterSlow();
        }

        public void AddPlayerCard(CardName cardName)
        {
            if (cardName == CardName.NONE) return;

            Card card = new Card(cardName);
            player.AddCard(card);
            deck.RemoveCard(cardName);
            probCounter.UpdateStats(player.CurrentHand, dealer.CurrentHand, deck);
        }

        public void AddDealerCard(CardName cardName)
        {
            if (cardName == CardName.NONE) return;

            Card card = new Card(cardName);
            dealer.AddCard(card);
            deck.RemoveCard(cardName);
            probCounter.UpdateStats(player.CurrentHand, dealer.CurrentHand, deck);
        }
    }
}
