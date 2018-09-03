

#include "card.h"
#include <string>
#include <array>
#include <iostream>
#include <unordered_map>


void Card::initializeCardValues()
{
	for (int i = 0; i < NUM_RANKS; ++i)
	{
		card_values[i] = i + 2;
	}
}

Card::Card (std::string in_name)
{
	initializeCardValues();
	if (in_name == "ACE" || in_name == "A")
	{
		name = ACE;
	}
	else if (in_name == "TWO" || in_name == "2")
	{
		name = TWO;
	}
	else if (in_name == "THREE" || in_name == "3")
	{
		name = THREE;
	}
	else if (in_name == "FOUR" || in_name == "4")
	{
		name = FOUR;
	}
	else if (in_name == "FIVE" || in_name == "5")
	{
		name = FIVE;
	}
	else if (in_name == "SIX" || in_name == "6")
	{
		name = SIX;
	}
	else if (in_name == "SEVEN" || in_name == "7")
	{
		name = SEVEN;
	}
	else if (in_name == "EIGHT" || in_name == "8")
	{
		name = EIGHT;
	}

	else if (in_name == "NINE" || in_name == "9")
	{
		name = NINE;
	}
	else if (in_name == "TEN" || in_name == "10")
	{
		name = TEN;
	}
	else
	{
		// ELSE THROW ERROR
	}

	card_name = in_name;
	value = card_values[name];
	// cout << "Card: " << card_name << ", value after initializer: " << value << "\n"; 
}

CardName Card::getName() 
{
	return name;
}

int Card::getValue() 
{
	return value;
}

void Card::setValue (int val) {
	if (val == 1 || val == 11) {
		value = val;
	}
	// otherwise throw an error (Ace is only card that changes)
	else {

	}
}
