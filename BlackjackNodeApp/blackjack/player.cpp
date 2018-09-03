#include "player.h"
#include "hand.h"
#include "card.h"
using namespace std;

// namespace BJ {

Player::Player() 
{
	hands.push_back(new Hand());
	curr_hand_index = 0;
}

void Player::addCard(Card *card) 
{
	if (shouldSplit())
	{
		hands.push_back(new Hand(card));
	}
	else 
	{
		hands[curr_hand_index]->addCard(card);
	}
}

int Player::getCurrentHandValue()
{
	return hands[curr_hand_index]->getTotalValue();
}

void Player::printCurrentHand() 
{
	hands[curr_hand_index]->printHand();
}

void Player::printHands()
{
	cout << "**** HAND START ****" << "\n";
	for (unsigned int i = 0; i < hands.size(); ++i)
	{
		cout << "Hand #" << i << "\n";
		for (unsigned int j = 0; j < hands[i]->cards.size(); ++j)
		{
			cout << hands[i]->cards[j]->card_name << "\n";
		}
	}
	cout << "**** HAND END ****" << "\n";
}

bool Player::shouldSplit() 
{
	return false;
}

// } // namespace BJ
