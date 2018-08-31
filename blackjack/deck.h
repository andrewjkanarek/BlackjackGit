#ifndef DECK_H
#define DECK_H

#include "ref.h"
#include <array>
#include <unordered_map>
// #include <map>
#include <iostream>
using namespace std;


class Card;

// namespace BJ {



class Deck {

public:

	// int card_counter[NUM_RANKS];
	array<int, NUM_RANKS> card_counter; // get rid of this

	// key: card_name (ex: "10")
	// value: # of cards with that name left in the deck
	// std::unordered_map<std::string, int> deck_map;
	std::unordered_map<std::string, string> deck_map_name;
	std::unordered_map<std::string, int> deck_map;


	int total_cards;


	Deck();
	~Deck();

	Deck(int num_decks);

	void removeCard(Card *card); 

	void printDeck();

	
};

// } // namespace BJ

#endif