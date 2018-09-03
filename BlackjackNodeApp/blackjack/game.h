// game.h
#ifndef GAME_H
#define GAME_H

// #include "deck.h"
// #include "player.h"
// #include "hand.h"
// #include "card.h"
#include <node.h>
#include <node_object_wrap.h>

class Deck;
class Player;
class Hand;
class Card;

// namespace BJ {
// namespace demo {



class Game : public node::ObjectWrap {
public:
	static void Init(v8::Local<v8::Object> exports);
	Deck *deck;
	Player *player;
	Hand *dealer_hand;
	int value;

private:
	explicit Game();
	explicit Game(int num_decks_in);
	~Game();

	static void New(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void AddPlayerCard(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void AddDealerCard(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void RemoveCard(const v8::FunctionCallbackInfo<v8::Value>& args); 
	static void UpdatePlayerStats(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void GetCurrentHandValue(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void GetCurrentHandCards(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void GetPlayerObject(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void GetDealerHandObject(const v8::FunctionCallbackInfo<v8::Value>& args);
	static void GetDeckObject(const v8::FunctionCallbackInfo<v8::Value>& args); 
	static v8::Persistent<v8::Function> game_constructor;

	// helper functions
	static v8::Local<v8::Object> GetV8CardObject(v8::Isolate* isolate, std::string card_name, int card_value);
	static v8::Local<v8::Object> GetV8HandObject(v8::Isolate* isolate, Game *obj, int hand_index);
	static v8::Handle<v8::Array> GetV8CardArray(v8::Isolate* isolate, std::vector<Card*> cards);
	static v8::Handle<v8::Array> GetV8HandArray(v8::Isolate* isolate, Game *obj, std::vector<Hand*> hands);
	static void PrintCurrentHand(Game *obj);
	static void PrintPlayerHands(Game *obj);
	static void PrintHandsVector(std::vector<Hand*> hands);	
	static void PrintCardsVector(std::vector<Card*> cards);

	
};

// } // namspace BJ

#endif