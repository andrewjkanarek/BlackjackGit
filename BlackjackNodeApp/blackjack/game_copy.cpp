// game.cpp
#include "game_copy.h"
#include "deck.h"
// #include "player.h"
#include "stats.h"
// #include "card.h"


Game::Game() 
{
  value = 0;
  int num_decks = 1;
  deck = new Deck(num_decks);
  deck->printDeck();
  player = new Player();
}


Game::~Game() { }

void Game::PrintCurrentHand(Game *obj)
{
  obj->player->printCurrentHand();
}

void Game::PrintPlayerHands(Game *obj)
{
  obj->player->printHands();
}

void Game::PrintHandsVector(vector<Hand*> hands)
{
  cout << "PRINTING HANDS\n";
  for (unsigned int i = 0; i < hands.size(); ++i)
  {
    cout << "Hand #" << i << "\n"; 
    for (unsigned int j = 0; j < hands[i]->cards.size(); ++j)
    {
      cout << hands[i]->cards[j]->card_name << "\n";
    }
  }
}

void Game::PrintCardsVector(vector<Card*> cards)
{
  cout << "PRINTING CARDS\n";
  for (unsigned int i = 0; i < cards.size(); ++i)
  {
    cout << cards[i]->card_name << "\n";
  }
}

