// game.h
#ifndef GAME_H
#define GAME_H

#include "deck.h"
#include "player.h"
#include "hand.h"
#include "card.h"

class Game {
public:
	Game();
	~Game();
	Deck *deck;
	Player *player;
	int value;

private:


	void PrintCurrentHand(Game *obj);
	void PrintPlayerHands(Game *obj);
	void PrintHandsVector(std::vector<Hand*> hands);	
	void PrintCardsVector(std::vector<Card*> cards);

	
};

// } // namspace BJ

#endif