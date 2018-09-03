#ifndef STATS_H
#define STATS_H

#include "hand.h"
#include <iostream>
#include <unordered_map>
using namespace std;

// namespace BJ {

// class Hand;

class Stats {
public:

	string decision;
	double dealer_bust;

	// "ah" stands for "after hitting"
	double win_ah;
	double lose_ah;
	double push_ah;

	double win;
	double lose;
	double push;

	// key: number str, value: probability of the dealer getting that number
	unordered_map<string, double> 
	dealer_prob_map 
	{ 
		{ "17", 0}, 
		{ "18", 0},
		{ "19", 0},
		{ "20", 0},
		{ "21", 0},
		{ "bust", 0}
	};

	// key: number str, value: probability of the dealer getting that number
	unordered_map<string, double> 
	player_prob_map 
	{ 
		{ "17", 0}, 
		{ "18", 0},
		{ "19", 0},
		{ "20", 0},
		{ "21", 0},
		{ "no_bust", 0}
	};

	Stats();

	// WIP
	// updates member variables based on new object instances
	void updateStats(Hand *player_hand, Deck *deck, Hand *dealer_hand);

private:

	void resetStats();

	// WIP
	double getProbDealerBust(Hand hand_in, Deck deck); // NOT USED

	// WIP
	double getProbWin(Hand *player_hand, Deck *deck, Hand *dealer_hand);
	double getProbLose(Hand *player_hand, Deck *deck, Hand *dealer_hand);
	double getProbPush(Hand *player_hand, Deck *deck, Hand *dealer_hand);

	double getProbWinAH();
	double getProbLoseAH();
	double getProbPushAH();

	// WIP
	string getDecision(int hand_value);	

	void setDealerProbMap1D(Hand hand_in, Deck deck_in, double prob);
	void setPlayerProbMap1D(Hand hand_in, Deck deck_in, double prob);

	double getProbOfCard(Deck deck, string card_name);

	double getProbNumber(Hand hand_in, Deck deck_in, int target_num);

	double getProbPlayerHigher(int player_value);
	double getProbDealerHigher(int player_value);

	double getProbPlayerHigherAH();
	double getProbDealerHigherAH();

	void printDealerMap1d();
	void printPlayerMap1d();

	// void updateHandAndDeck(Hand &hand, Deck &deck, string card_name);

};

// } // namespace BJ

#endif