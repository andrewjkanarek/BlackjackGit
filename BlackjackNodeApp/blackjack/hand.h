#ifndef HAND_H
#define HAND_H

#include "card.h"
#include <vector>
#include <iostream>
using namespace std;

// namespace BJ {

// class Card;
class Stats;
class StatsFast;
class Deck;

class Hand {

private:
	

public:
	int total_val;
	bool has_high_ace;
	int high_ace_index;
	vector<Card*> cards;
	Stats *stats;
	// StatsFast *stats;
	// StatsFast *stats_fast;
	bool can_split;

	Hand();

	Hand(Card *card);

	void addCard(Card *card);

	int getTotalValue();

	void printHand();

	void updateStats(Deck *deck, Hand *player_hand, Hand *dealer_hand);
};

// } // namespace BJ

#endif