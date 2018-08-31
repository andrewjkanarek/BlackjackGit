#ifndef PLAYER_H
#define PLAYER_H

#include <vector>
#include <iostream>
using namespace std;

// namespace BJ {

class Hand;
class Card;

class Player {
private:


public:
	vector<Hand*> hands;
	int curr_hand_index;

	Player();

	void addCard(Card *card);

	int getCurrentHandValue();

	void printCurrentHand();

	void printHands();

	bool shouldSplit();

	
};

// } // namespace BJ

#endif