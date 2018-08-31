
#include "deck.h"
#include "card.h"
#include <array>
#include <unordered_map>
#include <iostream>
using namespace std;

// namespace BJ {


Deck::Deck() { }

Deck::~Deck() { }

Deck::Deck(int num_decks) 
{
	total_cards = CARDS_PER_DECK * num_decks;

	deck_map.insert({"A", 4 * num_decks});
	deck_map_name.insert({"A", "ACE"});
	deck_map.insert({"10", 16 * num_decks});
	deck_map_name.insert({"10", "TEN"});
	deck_map.insert({"9", 4 * num_decks});
	deck_map_name.insert({"9", "NINE"});
	deck_map.insert({"8", 4 * num_decks});
	deck_map_name.insert({"8", "EIGHT"});
	deck_map.insert({"7", 4 * num_decks});
	deck_map_name.insert({"7", "SEVEN"});
	deck_map.insert({"6", 4 * num_decks});
	deck_map_name.insert({"6", "SIX"});
	deck_map.insert({"5", 4 * num_decks});
	deck_map_name.insert({"5", "FIVE"});
	deck_map.insert({"4", 4 * num_decks});
	deck_map_name.insert({"4", "FOUR"});
	deck_map.insert({"3", 4 * num_decks});
	deck_map_name.insert({"3", "THREE"});
	deck_map.insert({"2", 4 * num_decks});
	deck_map_name.insert({"2", "TWO"});

	card_counter[TWO] = 4 * num_decks;
	card_counter[THREE] = 4 * num_decks;
	card_counter[FOUR] = 4 * num_decks;
	card_counter[FIVE] = 4 * num_decks;
	card_counter[SIX] = 4 * num_decks;
	card_counter[SEVEN] = 4 * num_decks;
	card_counter[EIGHT] = 4 * num_decks;
	card_counter[NINE] = 4 * num_decks;
	card_counter[TEN] = 16 * num_decks;
	card_counter[ACE] = 4 * num_decks;
}

void Deck::removeCard(Card *card) 
{
	if (card_counter[card->getName()] > 0) 
	{	
		card_counter[card->getName()]--;
		if (deck_map.find(card->card_name) == deck_map.end())
		{
			cout << "Deck could not remove card. " 
				 << "Card " << card->card_name << "not found\n";
			// throw error
		}
		else
		{
			deck_map[card->card_name]--;
			total_cards--;
		}
	}
	//otherwise throw an exception
	else {

	}
}


void Deck::printDeck() 
{
	cout << "********* DECK *********\n";
	cout << "total #cards: " << total_cards << "\n";
	for (auto it = deck_map.begin(); it != deck_map.end(); ++it)
	{
		cout << it->first << ": " << it->second << "\n";
	}
}

	
// } // namespace BJ
