#ifndef CARD_H
#define CARD_H

#include "ref.h"
#include <string>
#include <array>
#include <iostream>
#include <unordered_map>


class Card {
public:
	int value;
	CardName name;
	int card_values[NUM_RANKS];
	std::string card_name;

	void initializeCardValues();

	Card (std::string in_name);

	CardName getName();

	int getValue();

	void setValue (int val);
};

#endif