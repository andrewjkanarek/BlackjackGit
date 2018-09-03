#include "deck.h"
#include "stats.h"
#include <iostream>
// #include <unordered_map>
using namespace std;

// namespace BJ {

Stats::Stats() 
{
	decision = "HIT";
	resetStats();
}

void Stats::resetStats()
{
	dealer_bust = 0;
	win_ah = 0;
	lose_ah = 0;
	push_ah = 0;
	win = 0;
	lose = 0;
	push = 0;
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

// WIP
void Stats::updateStats(Hand *player_hand, Deck *deck, Hand *dealer_hand)
{
	resetStats();
	// printDealerMap1d();
	setDealerProbMap1D(*dealer_hand, *deck, 1);
	// printDealerMap1d();
	setPlayerProbMap1D(*player_hand, *deck, 1);
	// printPlayerMap1d();

	dealer_bust = dealer_prob_map["bust"];
	// get the probability of winning without doing anything
	win = getProbWin(player_hand, deck, dealer_hand);
	// cout << "prob win: " << win << "\n";
	// get the probability of losing without doing anything
	lose = getProbLose(player_hand, deck, dealer_hand);
	// cout << "prob lose: " << lose << "\n";
	// get the probability of pushing without doing anything
	push = getProbPush(player_hand, deck, dealer_hand);

	win_ah = getProbWinAH();

	lose_ah = getProbLoseAH();

	push_ah = getProbPushAH();

	decision = getDecision(player_hand->total_val);
}

double Stats::getProbWin(Hand *player_hand, Deck *deck, Hand *dealer_hand)
{

	double prob_win = 0;
	if (player_hand->total_val < 17)
	{
		prob_win = dealer_prob_map["bust"];
	}
	else if (player_hand->total_val > 21)
	{
		prob_win = 0;
	}
	else
	{
		prob_win = getProbPlayerHigher(player_hand->total_val) + dealer_prob_map["bust"];
	}
	if (prob_win < 0.000001)
		return 0;
	return prob_win;
}


// WIP
double Stats::getProbLose(Hand *player_hand, Deck *deck, Hand *dealer_hand)
{
	double prob_lose = 0;
	if (player_hand->total_val < 17)
	{
		prob_lose = 1 - dealer_prob_map["bust"];
	}
	else
	{
		prob_lose = getProbDealerHigher(player_hand->total_val);
	}
	if (prob_lose < 0.000001)
		return 0;
	return prob_lose;
}

// WIP
double Stats::getProbPush(Hand *player_hand, Deck *deck, Hand *dealer_hand)
{	
	// cout << "entering push function\n";
	// cout << "prob dealer gets 17: " << dealer_prob_map["17"] << "\n";
	if (player_hand->total_val < 17)
	{
		return 0;
	}
	else if (player_hand->total_val == 17)
	{
		return dealer_prob_map["17"];
	}
	else if (player_hand->total_val == 18)
	{
		return dealer_prob_map["18"];
	}
	else if (player_hand->total_val == 19)
	{
		return dealer_prob_map["19"];
	}
	else if (player_hand->total_val == 20)
	{
		return dealer_prob_map["20"];
	}
	else if (player_hand->total_val == 21)
	{
		return dealer_prob_map["21"];
	}
	// cout << "HERE2\n";
	return 0;
}

// the probability of player winning:
//		a) dealer busts AND player doesn't bust
//		OR
//		b) player>dealer and player doesn't bust
double Stats::getProbWinAH()
{
	double p = 0;
	double a = dealer_prob_map["bust"] * player_prob_map["no_bust"];
	double b = getProbPlayerHigherAH();
	// cout << "prob dealer bust and player doesn't\n";
	// cout << "getProbPlayerHigherAH: " << b << "\n";
	p = a + b;
	return p;

}

// the probability of player winning:
//		a) player busts
//		OR
//		b) dealer>player and dealer doesn't bust
double Stats::getProbLoseAH()
{
	double p = 0;
	double a = 1 - player_prob_map["no_bust"];
	double b = getProbDealerHigherAH();
	// cout << "b: " << b << "\n";
	p = a + b;
	return p;
}

double Stats::getProbPushAH()
{
	double p = 0;

	p += (dealer_prob_map["17"] * player_prob_map["17"]);
	p += (dealer_prob_map["18"] * player_prob_map["18"]);
	p += (dealer_prob_map["19"] * player_prob_map["19"]);
	p += (dealer_prob_map["20"] * player_prob_map["20"]);
	p += (dealer_prob_map["21"] * player_prob_map["21"]);

	return p;
}

// WIP
string Stats::getDecision(int hand_value)
{
	double win_diff = win - lose;
	double win_diff_ah = win_ah - lose_ah;
	if (hand_value > 21)
	{
		return "BUSTED";
	}
	else if (win_diff > win_diff_ah)
	{
		return "STAND";
	}
	else if (win_diff < win_diff_ah)
	{
		return "HIT";
	}
	else
	{
		return "IT'S NOT UP TO YOU. IT'S UP TO GOD";
	}
	
}

void Stats::setDealerProbMap1D(Hand hand_in, Deck deck_in, double prob)
{
	int dealer_total = hand_in.total_val;
	if (dealer_total > 21)
	{
		dealer_prob_map["bust"] = (dealer_prob_map["bust"] == 0) ? prob : (dealer_prob_map["bust"] += prob);
	}
	else if (dealer_total == 17)
	{
		dealer_prob_map["17"] = (dealer_prob_map["17"] == 0) ? prob : (dealer_prob_map["17"] += prob);
	}
	else if (dealer_total == 18)
	{
		dealer_prob_map["18"] = (dealer_prob_map["18"] == 0) ? prob : (dealer_prob_map["18"] += prob);
	}
	else if (dealer_total == 19)
	{
		dealer_prob_map["19"] = (dealer_prob_map["19"] == 0) ? prob : (dealer_prob_map["19"] += prob); 
	}
	else if (dealer_total == 20)
	{
		dealer_prob_map["20"] = (dealer_prob_map["20"] == 0) ? prob : (dealer_prob_map["20"] += prob); 
	}
	else if (dealer_total == 21)
	{
		dealer_prob_map["21"] = (dealer_prob_map["21"] == 0) ? prob : (dealer_prob_map["21"] += prob);
	}
	else
	{

		for (auto it = deck_in.deck_map.begin(); it != deck_in.deck_map.end(); it++)
		{
			int remaining = it->second;
			if (remaining > 0)
			{
				string card_name = it->first;
				double prob_of_card = getProbOfCard(deck_in, card_name) * prob;
				Card *card = new Card(card_name);
				Hand hand = hand_in;
				hand.addCard(card);
				Deck deck = deck_in;
				deck.removeCard(card);
				setDealerProbMap1D(hand, deck, prob_of_card);
			}
		}

	}
}

void Stats::setPlayerProbMap1D(Hand hand_in, Deck deck_in, double prob)
{

	int dealer_total = hand_in.total_val;
	if (dealer_total > 21)
	{
		return;
	}

	for (auto it = deck_in.deck_map.begin(); it != deck_in.deck_map.end(); it++)
	{
		int remaining = it->second;
		if (remaining > 0)
		{
			string card_name = it->first;
			double prob_of_card = getProbOfCard(deck_in, card_name) * prob;
			Card *card = new Card(card_name);
			Hand hand = hand_in;
			hand.addCard(card);
			Deck deck = deck_in;
			deck.removeCard(card);
			if (hand.total_val >= 17 && hand.total_val <= 21)
			{
				player_prob_map["no_bust"] = (player_prob_map["no_bust"] == 0) ? prob_of_card : (player_prob_map["no_bust"] += prob_of_card);
				if (hand.total_val == 17)
				{
					player_prob_map["17"] = (player_prob_map["17"] == 0) ? prob_of_card : (player_prob_map["17"] += prob_of_card);
				}
				else if (hand.total_val == 18)
				{
					player_prob_map["18"] = (player_prob_map["18"] == 0) ? prob_of_card : (player_prob_map["18"] += prob_of_card);
				}
				else if (hand.total_val == 19)
				{
					player_prob_map["19"] = (player_prob_map["19"] == 0) ? prob_of_card : (player_prob_map["19"] += prob_of_card);
				}
				else if (hand.total_val == 20)
				{
					player_prob_map["20"] = (player_prob_map["20"] == 0) ? prob_of_card : (player_prob_map["20"] += prob_of_card);
				}
				else if (hand.total_val == 21)
				{
					player_prob_map["21"] = (player_prob_map["21"] == 0) ? prob_of_card : (player_prob_map["21"] += prob_of_card);
				}
			}
			
			setPlayerProbMap1D(hand, deck, prob_of_card);
		}
	}


}

double Stats::getProbPlayerHigherAH()
{
	double prob = 0;
	prob += ( dealer_prob_map["17"] *  
		(player_prob_map["18"] + player_prob_map["19"] + player_prob_map["20"] + player_prob_map["21"]) );
	prob += ( dealer_prob_map["18"] *  
		(player_prob_map["19"] + player_prob_map["20"] + player_prob_map["21"]) );
	prob += ( dealer_prob_map["19"] *  (player_prob_map["20"] + player_prob_map["21"]) );
	prob += ( dealer_prob_map["20"] *  player_prob_map["21"] );
	return prob;
}

double Stats::getProbDealerHigherAH()
{
	double prob = 0;
	prob += ( (player_prob_map["17"]) *  
		(dealer_prob_map["18"] + dealer_prob_map["19"] + dealer_prob_map["20"] + dealer_prob_map["21"]) );
	prob += ( (player_prob_map["18"]) *  
		(dealer_prob_map["19"] + dealer_prob_map["20"] + dealer_prob_map["21"]) );
	prob += ( (player_prob_map["19"]) *  (dealer_prob_map["20"] + dealer_prob_map["21"]) );
	prob += ( player_prob_map["20"] *  dealer_prob_map["21"] );
	return prob;
}

double Stats::getProbOfCard(Deck deck, string card_name)
{
	// probability of a card = (number of that card in the deck / number of cards in the deck)
	int total_cards = deck.total_cards;
	int num = deck.deck_map[card_name];
	// cout << "num: " << num << ", total: " << total_cards << "\n";
	return float(num) / total_cards;
}

/* NOT USED ANYMORE */
double Stats::getProbDealerBust(Hand hand_in, Deck deck_in)
{
	// cout << "entering getProbDealerBust\n";
	int total_value = hand_in.total_val;
	// cout << "dealer value: " << total_value << "\n";
	// if the dealer's total is between min and max, delaer cannot hit and cannot bust
	if (total_value >= DEALER_MIN && total_value <= DEALER_MAX) 
	{
		return 0;
	}
	double prob = 0;
	// for each possible card in the deck
	for (auto it = deck_in.deck_map.begin(); it != deck_in.deck_map.end(); ++it)
	{
		// cout << "current card: " << it->first << "\n";
		int remaining = it->second;
		// check if there are any left in the deck
		if (remaining > 0)
		{
			string card_name = it->first;
			double prob_of_card = getProbOfCard(deck_in, card_name); 
			Card *card = new Card(card_name);
			// add the card to the current hand
			Hand hand = hand_in;
			hand.addCard(card);
			// remove the card from the deck
			Deck deck = deck_in;
			deck.removeCard(card);

			// if the card causes bust, add the probability of getting that card
			if (hand.total_val > 21)
			{
				prob += prob_of_card;
				// cout << "Cards left in deck: " << deck.total_cards << "\n";
			}
			// if the card doesn't cause a bust, 
			// add (probabiliy of getting that card * probability of busting with that card
			else
			{
				prob += prob_of_card * getProbDealerBust(hand, deck);
			}

		}

	}

	return prob;
}

double Stats::getProbNumber(Hand hand_in, Deck deck_in, int target_num)
{
	if (hand_in.total_val > target_num)
	{
		return 0;
	}
	else if (hand_in.total_val < target_num)
	{
		double prob = 0;
		for (auto it = deck_in.deck_map.begin(); it != deck_in.deck_map.end(); ++it)
		{
			int remaining = it->second;
			if (remaining > 0)
			{
				string card_name = it->first;
				double prob_of_card = getProbOfCard(deck_in, card_name);
				// cout << "card: " << card_name << "\n";
				Card *card = new Card(card_name);
				// add the card to the current hand
				Hand hand = hand_in;
				hand.addCard(card);
				// remove the card from the deck
				Deck deck = deck_in;
				deck.removeCard(card);
				// cout << "prob of card: " << getProbOfCard(deck, card_name) << "\n";
				// cout << "prob number: " << getProbNumber(hand, deck, target_num) << "\n";
				prob += prob_of_card * getProbNumber(hand, deck, target_num);
				// cout << "new prob: " << prob << "\n";
			}
		}
		return prob;

	}
	else 
	{
		return 1;
	}
}


double Stats::getProbPlayerHigher(int player_value)
{
	if (player_value == 18)
	{
		return dealer_prob_map["17"];
	}
	else if (player_value == 19)
	{
		return dealer_prob_map["17"] + dealer_prob_map["18"];
	}
	else if (player_value == 20)
	{
		return dealer_prob_map["17"] + dealer_prob_map["18"] + dealer_prob_map["19"];
	}
	else if (player_value == 21)
	{
		return dealer_prob_map["17"] + dealer_prob_map["18"] + dealer_prob_map["19"] + dealer_prob_map["20"];
	}
	return 0;
}

double Stats::getProbDealerHigher(int player_value)
{
	if (player_value == 20)
	{
		return dealer_prob_map["21"];
	}
	else if (player_value == 19)
	{
		return dealer_prob_map["20"] + dealer_prob_map["21"];
	}
	else if (player_value == 18)
	{
		return dealer_prob_map["19"] + dealer_prob_map["20"] + dealer_prob_map["21"];
	}
	else if (player_value == 17)
	{
		return dealer_prob_map["18"] + dealer_prob_map["19"] + dealer_prob_map["20"] + dealer_prob_map["21"];
	}
	return 0;
}


void Stats::printDealerMap1d()
{
	cout << "*** Dealer Map ***\n";
	for (auto it = dealer_prob_map.begin(); it != dealer_prob_map.end(); it++)
	{
		cout << it->first << ": " << it->second << "\n";
	}
}

void Stats::printPlayerMap1d()
{
	cout << "*** Player Map ***\n";
	for (auto it = player_prob_map.begin(); it != player_prob_map.end(); it++)
	{
		cout << it->first << ": " << it->second << "\n";
	}
}



// } // namespace std;	
