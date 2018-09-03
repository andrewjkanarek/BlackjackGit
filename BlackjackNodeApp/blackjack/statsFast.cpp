#include "deck.h"
#include "statsFast.h"
#include <iostream>
#include <string>
// #include <unordered_map>
using namespace std;

// namespace BJ {

StatsFast::StatsFast() 
{
	decision = "HIT";
	resetStats();
}

void StatsFast::resetStats()
{
	dealer_bust = 0;
	win_ah = 0;
	lose_ah = 0;
	push_ah = 0;
	win = 0;
	lose = 0;
	push = 0;
	probArray = {};
	player_prob_map =
	{ 
		{ "17", 0}, 
		{ "18", 0},
		{ "19", 0},
		{ "20", 0},
		{ "21", 0},
		{ "no_bust", 0}
	};
	dealer_prob_map =
	{ 
		{ "17", 0}, 
		{ "18", 0},
		{ "19", 0},
		{ "20", 0},
		{ "21", 0},
		{ "bust", 0}
	};
}

void StatsFast::updateStats(Hand *player_hand, Deck *deck, Hand *dealer_hand)
{
	initializeProbArray(*deck);
	printProbArray();
	cout << "SUM: " << getProbArraySum() << "\n";

	setDealerProbMap1D(*dealer_hand);
	printDealerMap1d();

	win = getProbWin(player_hand, deck, dealer_hand);

	decision = "HIT";
	dealer_bust = 0;
	win_ah = 0;
	lose_ah = 0;
	push_ah = 0;
	lose = 0;
	push = 0;
}

void StatsFast::initializeProbArray(Deck deck)
{
	for (unsigned int i = 0; i < probArray.size(); ++i)
	{
		if (i == 0)
		{
			probArray[0] = 0;
		}
		else if (i == 1)
		{
			probArray[1] = getProbOfCard(deck, "A");
		} 
		else
		{
			int start_index;
			// if current index is even
			// start as the index / 2
			if (i % 2 == 0)
			{
				start_index = i / 2;
			}
			// otherwise start at (index / 2) + 1
			else
			{
				start_index = (i / 2) + 1;
			}
			double sum = 0;
			for (unsigned int j = start_index; j < i; j++)
			{
				sum += probArray[j] * probArray[i-j];
			}
			if (i <= 11)
			{
				sum += getProbOfCard(deck, convertIntToCardName(i));
			}
			probArray[i] = sum;
		}
	}
}

double StatsFast::getProbWin(Hand *player_hand, Deck *deck, Hand *dealer_hand)
{
	double prob = probArray[1];
	return prob;
}

double StatsFast::getProbOfCard(Deck deck, string card_name)
{
	// probability of a card = (number of that card in the deck / number of cards in the deck)
	int total_cards = deck.total_cards;
	int num = deck.deck_map[card_name];
	// cout << "num: " << num << ", total: " << total_cards << "\n";
	return float(num) / total_cards;
}

string StatsFast::convertIntToCardName(int val)
{
	if (val == 1 || val == 11)
	{
		return "A";
	}
	else 
	{
		return to_string(val);
	}
}

void StatsFast::setDealerProbMap1D(Hand dealer_hand)
{
	// if the dealer hand is in the range the dealer must stick, then the dealer's value 
	// will definitely end as that value
	//	ex: if a dealer has an "18", the dealer must stick and will end with an "18"
	if (dealer_hand.total_val >= DEALER_MIN && dealer_hand.total_val <= DEALER_MAX)
	{
		dealer_prob_map[to_string(dealer_hand.total_val)] = 1;
	}
	// otherwise, the probability of getting a value is the probability of getting a number
	// that adds up to that value
	// ex: prob of getting a "17" if the dealer has a "5" is the probability of getting a "12"
	// probArray(n) gets the probability of getting "n" after unlimited hits
	else if (dealer_hand.total_val < DEALER_MIN)
	{
		dealer_prob_map["17"] = probArray[17 - dealer_hand.total_val];
		dealer_prob_map["18"] = probArray[18 - dealer_hand.total_val];
		dealer_prob_map["19"] = probArray[19 - dealer_hand.total_val];
		dealer_prob_map["20"] = probArray[20 - dealer_hand.total_val];
		dealer_prob_map["21"] = probArray[21 - dealer_hand.total_val];
	}
	else
	{
		for (unsigned int i = DEALER_MAX - (dealer_hand.total_val - 1); i <= 11; ++i)
		{
			dealer_prob_map["bust"] += probArray[i];
		}
	}
}

void StatsFast::setPlayerProbMap1D(Hand player_hand)
{

}

void StatsFast::printProbArray()
{
	for (unsigned int i = 0; i < probArray.size(); ++i)
	{
		cout << i << ": " << probArray[i] << "\n";
	}
}

void StatsFast::printDealerMap1d()
{
	cout << "*** Dealer Map ***\n";
	for (auto it = dealer_prob_map.begin(); it != dealer_prob_map.end(); it++)
	{
		cout << it->first << ": " << it->second << "\n";
	}
}

void StatsFast::printPlayerMap1d()
{
	cout << "*** Player Map ***\n";
	for (auto it = player_prob_map.begin(); it != player_prob_map.end(); it++)
	{
		cout << it->first << ": " << it->second << "\n";
	}
}

double StatsFast::getProbArraySum()
{
	double sum = 0;
	for (unsigned int i = 0; i < probArray.size(); ++i)
	{
		sum += probArray[i];
	}
	return sum;
}

