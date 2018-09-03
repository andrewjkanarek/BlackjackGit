
#include "hand.h"
#include "stats.h"
#include "statsFast.h"
#include "deck.h"
#include <vector>
#include <iostream>
using namespace std;

// namespace BJ {

Hand::Hand() 
{
	total_val = 0;
	has_high_ace = false;
	stats = new Stats();
	// stats = new StatsFast();
	// stats_fast = new StatsFast();
	can_split = false;
};

Hand::Hand(Card *card) 
{
	total_val = 0;
	has_high_ace = false;
	high_ace_index = -1;
	addCard(card);
	stats = new Stats();
	// stats = new StatsFast();
	// stats_fast = new StatsFast();
	can_split = false;
};

void Hand::addCard(Card *card) 
{
	if (card->getName() == ACE) 
	{
		has_high_ace = true;
		high_ace_index = cards.size();
	}
	total_val += card->getValue();
	cards.push_back(card);

	if (total_val > 21 && has_high_ace)
	{
		cards[high_ace_index]->setValue(1);
		has_high_ace = false;
		high_ace_index = -1;
		total_val -= 10;
	}

	// check if the hand can be split
	// hand can be split if it contains two cards that are the same
	if (cards.size() == 2 && cards[0]->card_name == cards[1]->card_name)
	{
		can_split = true;
	}

}

int Hand::getTotalValue()
{
	return total_val;
}

void Hand::printHand() {
	cout << "*** CURRENT HAND ***\n";
	cout << "total value: " << total_val << "\n";
	for (unsigned int i = 0; i < cards.size(); ++i) {
		cout << cards[i]->getName() << " ";
	}
	cout << "\n";
}

void Hand::updateStats(Deck *deck, Hand *player_hand, Hand *dealer_hand)
{
	stats->updateStats(player_hand, deck, dealer_hand);
	// stats_fast->updateStats(player_hand, deck, dealer_hand);
	cout << "new prob of win: " << stats->win << "\n";
}

// } // namespace BJ
