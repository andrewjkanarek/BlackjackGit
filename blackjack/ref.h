#ifndef REF_H
#define REF_H

#include <set>
#include <unordered_map>
using namespace std;

const int NUM_RANKS = 10;
const int DEALER_MIN = 17;
const int DEALER_MAX = 21;
const int CARDS_PER_DECK = 52;

const std::set<std::string> valid_card_names; //= { "ACE", "TEN", "NINE", "EIGHT", "SEVEN", "SIX", "FIVE", "FOUR", "THREE", "TWO" };
// valid_card_names.insert("ACE");




enum CardName {
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
};

enum Decision {
	HIT,
	STAY,
	SPLIT,
	DOUBLE,
	BUST,
	PUSH
};


#endif