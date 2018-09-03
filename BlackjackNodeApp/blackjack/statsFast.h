#ifndef STATSFAST_H
#define STATSFAST_H

#include "ref.h"
#include "hand.h"
#include <iostream>

using namespace std;


class StatsFast {
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

	/*
	array will be dynamically filled to hold the probabilities of getting each amount
		ex: probArray[14] = probability of getting a 14
	size = DEALER_MAX + 1 to make index start at 1 so
		index = 0 = 0 (not used)
		index = 1 = A
		index = 2 = 2
		etc...
	*/
	array<double, DEALER_MAX + 1> probArray;

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

	StatsFast();



	// updates member variables based on new object instances
	void updateStats(Hand *player_hand, Deck *deck, Hand *dealer_hand);

private:

	void resetStats();

	void initializeProbArray(Deck deck);

	double getProbWin(Hand *player_hand, Deck *deck, Hand *dealer_hand);

	double getProbOfCard(Deck deck, string card_name);

	// temporary solution. Fix to more clean/efficient solution
	string convertIntToCardName(int val);

	void setDealerProbMap1D(Hand dealer_hand);
	void setPlayerProbMap1D(Hand player_hand);
	void printPlayerMap1d();
	void printDealerMap1d();

	void printProbArray();
	double getProbArraySum();


};


#endif