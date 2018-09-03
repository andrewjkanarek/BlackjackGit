// game.cpp
#include "game.h"
#include "deck.h"
#include "player.h"
#include "stats.h"
#include "statsFast.h"
#include "card.h"
#include "hand.h"
#include <array>
#include <node.h>
#include <node_object_wrap.h>

// namespace BJ {
// namespace demo {

using v8::Context;
using v8::Function;
using v8::FunctionCallbackInfo;
using v8::FunctionTemplate;
using v8::Isolate;
using v8::Local;
using v8::Number;
using v8::Object;
using v8::Persistent;
using v8::String;
using v8::Value;

Persistent<Function> Game::game_constructor;

Game::Game() 
{
  value = 0;
  int num_decks = 1;
  deck = new Deck(num_decks);
  player = new Player();
  dealer_hand = new Hand();
  // deck->printDeck();
}

Game::Game(int num_decks_in) 
{
  value = 0;
  int num_decks = num_decks_in;
  deck = new Deck(num_decks);
  deck = new Deck(num_decks);
  player = new Player();
  dealer_hand = new Hand();
}
// {
// 	// int num_decks = 1;
// }

Game::~Game() { }

void Game::Init(Local<Object> exports) 
{
  Isolate* isolate = exports->GetIsolate();

  // Prepare constructor template
  Local<FunctionTemplate> tpl = FunctionTemplate::New(isolate, New);
  tpl->SetClassName(String::NewFromUtf8(isolate, "Game"));
  tpl->InstanceTemplate()->SetInternalFieldCount(1);

  // Prototype
  NODE_SET_PROTOTYPE_METHOD(tpl, "addPlayerCard", AddPlayerCard);
  NODE_SET_PROTOTYPE_METHOD(tpl, "addDealerCard", AddDealerCard);
  NODE_SET_PROTOTYPE_METHOD(tpl, "removeCard", RemoveCard);
  NODE_SET_PROTOTYPE_METHOD(tpl, "updatePlayerStats", UpdatePlayerStats);
  NODE_SET_PROTOTYPE_METHOD(tpl, "getCurrentHandValue", GetCurrentHandValue);
  NODE_SET_PROTOTYPE_METHOD(tpl, "getCurrentHandCards", GetCurrentHandCards);
  NODE_SET_PROTOTYPE_METHOD(tpl, "getPlayerObject", GetPlayerObject);
  NODE_SET_PROTOTYPE_METHOD(tpl, "getDealerHandObject", GetDealerHandObject);
  NODE_SET_PROTOTYPE_METHOD(tpl, "getDeckObject", GetDeckObject);

  game_constructor.Reset(isolate, tpl->GetFunction());
  exports->Set(String::NewFromUtf8(isolate, "Game"),
               tpl->GetFunction());
}

void Game::New(const FunctionCallbackInfo<Value>& args) 
{
  Isolate* isolate = args.GetIsolate();

  if (args.IsConstructCall()) 
  {
    // Invoked as constructor: `new Game(...)`
    // double val = args[0]->IsUndefined() ? 0 : args[0]->NumberValue();
    Game* obj = new Game();
    obj->Wrap(args.This());
    args.GetReturnValue().Set(args.This());
  } 
  else 
  {
    // Invoked as plain function `Game(...)`, turn into construct call.
    const int argc = 1;
    Local<Value> argv[argc] = { args[0] };
    Local<Context> context = isolate->GetCurrentContext();
    Local<Function> cons = Local<Function>::New(isolate, game_constructor);
    Local<Object> result =
        cons->NewInstance(context, argc, argv).ToLocalChecked();
    args.GetReturnValue().Set(result);
  }
}


void Game::AddPlayerCard(const FunctionCallbackInfo<Value>& args) 
{
	Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());
  v8::String::Utf8Value param1(args[0]->ToString()); // get param in v8 string
  std::string card_name = std::string(*param1); // convert v8 string to std string
  // cout << "card name: " << card_name << "\n";
	Card *card = new Card(card_name);
	obj->deck->removeCard(card);
	obj->player->addCard(card);

  // obj->deck->printDeck();
  // PrintPlayerHands(obj);
}

void Game::AddDealerCard(const FunctionCallbackInfo<Value>& args) 
{
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());
  v8::String::Utf8Value param1(args[0]->ToString()); // get param in v8 string
  std::string card_name = std::string(*param1); // convert v8 string to std string
  // cout << "card name: " << card_name << "\n";
  Card *card = new Card(card_name);
  obj->deck->removeCard(card);
  obj->dealer_hand->addCard(card);

  // obj->deck->printDeck();
  // PrintPlayerHands(obj);
}

void Game::RemoveCard(const FunctionCallbackInfo<Value>& args) 
{
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());
  v8::String::Utf8Value param1(args[0]->ToString()); // get param in v8 string
  std::string card_name = std::string(*param1); // convert v8 string to std string
  // cout << "card name: " << card_name << "\n";
  Card *card = new Card(card_name);
  obj->deck->removeCard(card);
  cout << "deck removeCard called successfully\n";
  obj->deck->printDeck();
}

void Game::UpdatePlayerStats(const FunctionCallbackInfo<Value>& args)
{
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());
  int hand_index = args[0]->NumberValue();
  // update the stats based on new player hand and deck
  Hand *player_hand = obj->player->hands[hand_index];

  // if it is the player's first hand, the hand must have at least two cards
  // if it is not the first hand, it can have more 
  if ( (hand_index == 0 && player_hand->cards.size() >= 2 && obj->dealer_hand->cards.size() >= 1) ||
       (hand_index != 0 && player_hand->cards.size() >= 1 && obj->dealer_hand->cards.size() >= 1) )
  {
    player_hand->updateStats(obj->deck, player_hand, obj->dealer_hand);
  }
}

v8::Handle<v8::Array> Game::GetV8CardArray(Isolate* isolate, vector<Card*> cards)
{
  // copy vector of cards over to v8 array
  v8::Handle<v8::Array> v8_cards = v8::Array::New(isolate);

  // add each card name to v8 array 
  for (unsigned int i = 0; i < cards.size(); ++i)
  {
    // convert std::string to v8 string
    Local<Object> card = GetV8CardObject(isolate, cards[i]->card_name, cards[i]->value);
    v8_cards->Set(i, card);
  }

  return v8_cards;
}

Local<Object> Game::GetV8CardObject(Isolate* isolate, std::string card_name, int card_value)
{
  Local<Object> card = Object::New(isolate);
  Local<Value> card_name_key = v8::String::NewFromUtf8(isolate, "name");
  Local<Value> name = v8::String::NewFromUtf8(isolate, card_name.c_str());
  Local<Value> card_value_key = v8::String::NewFromUtf8(isolate, "value");
  Local<Value> value = Number::New(isolate, card_value);
  card->Set(card_name_key, name);
  card->Set(card_value_key, value);
  return card; 
}


/* 
the hand object consists of:
  an array of cards
  statistics
  total value
  decision of what the player should do
*/
Local<Object> Game::GetV8HandObject(Isolate* isolate, Game *obj, int hand_index)
{
  // declare new v8 object 
  Local<Object> hand_object = Object::New(isolate);

  // create key and value (v8 array) for cards in the hand
  Local<Value> cards_key = v8::String::NewFromUtf8(isolate, "cards");
  v8::Handle<v8::Array> v8_cards = obj->GetV8CardArray(isolate, obj->player->hands[hand_index]->cards);

  // create key and value (int) for total value of the hand
  Local<Value> total_value_key = v8::String::NewFromUtf8(isolate, "total_value");
  Local<Value> total_value = Number::New(isolate, obj->player->hands[hand_index]->total_val);

  // create key and value (double) for probability of winning without hitting
  Local<Value> win_key = v8::String::NewFromUtf8(isolate, "win");
  Local<Value> win = Number::New(isolate, obj->player->hands[hand_index]->stats->win);

  Local<Value> lose_key = v8::String::NewFromUtf8(isolate, "lose");
  Local<Value> lose = Number::New(isolate, obj->player->hands[hand_index]->stats->lose);

  Local<Value> push_key = v8::String::NewFromUtf8(isolate, "push");
  Local<Value> push = Number::New(isolate, obj->player->hands[hand_index]->stats->push);

  Local<Value> dealer_bust_key = v8::String::NewFromUtf8(isolate, "dealer_bust");
  Local<Value> dealer_bust = Number::New(isolate, obj->player->hands[hand_index]->stats->dealer_bust);

  Local<Value> win_ah_key = v8::String::NewFromUtf8(isolate, "win_ah");
  Local<Value> win_ah = Number::New(isolate, obj->player->hands[hand_index]->stats->win_ah);

  Local<Value> lose_ah_key = v8::String::NewFromUtf8(isolate, "lose_ah");
  Local<Value> lose_ah = Number::New(isolate, obj->player->hands[hand_index]->stats->lose_ah);

  Local<Value> push_ah_key = v8::String::NewFromUtf8(isolate, "push_ah");
  Local<Value> push_ah = Number::New(isolate, obj->player->hands[hand_index]->stats->push_ah);

  // create a key and value (string) for recommended decision
  Local<Value> decision_key = v8::String::NewFromUtf8(isolate, "decision");
  Local<Value> decision = v8::String::NewFromUtf8(isolate, obj->player->hands[hand_index]->stats->decision.c_str());

  Local<Value> split_key = v8::String::NewFromUtf8(isolate, "can_split");
  Local<Value> can_split = Number::New(isolate, obj->player->hands[hand_index]->can_split);

  // bool is_hand_done = (obj->player->hands[hand_index]->total_val > 21) ? false : true;
  // Local<Value> hand_status_key = v8::String::NewFromUtf8(isolate, "is_hand_done");
  // Local<Value> hand_status = v8::String::NewFromUtf8(isolate, obj->player->hands[hand_index]->stats->decision.c_str());

  // add key/value pairs to object
  hand_object->Set(cards_key, v8_cards);
  hand_object->Set(total_value_key, total_value);
  hand_object->Set(win_key, win);
  hand_object->Set(lose_key, lose);
  hand_object->Set(push_key, push);
  hand_object->Set(dealer_bust_key, dealer_bust);
  hand_object->Set(win_ah_key, win_ah);
  hand_object->Set(lose_ah_key, lose_ah);
  hand_object->Set(push_ah_key, push_ah);
  hand_object->Set(decision_key, decision);
  hand_object->Set(split_key, can_split);

  return hand_object;
}

v8::Handle<v8::Array> Game::GetV8HandArray(Isolate* isolate, Game *obj, vector<Hand*> hands)
{
  // declare new v8 hands array
  v8::Handle<v8::Array> v8_hands = v8::Array::New(isolate);

  // for each player's hand, get the hand object
  for (unsigned int i = 0; i < hands.size(); ++i)
  {
    // get vector of cards
    vector<Card*> cards = obj->player->hands[i]->cards;

    // get the JSON formatted object for the current hand
    Local<Object> hand_object = GetV8HandObject(isolate, obj, i);
    // add the current hand to the v8 array of hands
    v8_hands->Set(i, hand_object);

  }

  return v8_hands;
}


// NOT NEEDED ANYMORE
void Game::GetCurrentHandCards(const FunctionCallbackInfo<Value>& args) 
{
  Isolate* isolate = args.GetIsolate();
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());

  /* TOO MUCH COPYING. FIGURE OUT A WAY TO USE LESS MEMORY*/

  // get vector of cards
  vector<Card*> cards = obj->player->hands[obj->player->curr_hand_index]->cards;

  // copy vector over to v8 array
  v8::Handle<v8::Array> result = obj->GetV8CardArray(isolate, cards);

  // return vector to JS
  args.GetReturnValue().Set(result);

}


// return value should be a JS list of lists
// a list of cards for each player hand
void Game::GetPlayerObject(const FunctionCallbackInfo<Value>& args) 
{
  Isolate* isolate = args.GetIsolate();
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());

  // declare new v8 object 
  Local<Object> player_object = Object::New(isolate);

  // update the stats based on new player hand and deck ** this is done in a separate function now
  // Hand *curr_player_hand = obj->player->hands[obj->player->curr_hand_index];
  // curr_player_hand->updateStats(obj->deck, curr_player_hand, obj->dealer_hand); 
  
  /* TOO MUCH COPYING. FIGURE OUT A WAY TO USE LESS MEMORY*/

  // get vector of player hands
  vector<Hand*> hands = obj->player->hands;

  PrintHandsVector(hands);

  // create key and value (v8 array) for hands in the player
  Local<Value> hands_key = v8::String::NewFromUtf8(isolate, "hands");
  v8::Handle<v8::Array> v8_hands = GetV8HandArray(isolate, obj, hands);

  Local<Value> cur_hand_key = v8::String::NewFromUtf8(isolate, "curr_hand");
  Local<Value> curr_hand = Number::New(isolate, obj->player->curr_hand_index);

  player_object->Set(hands_key, v8_hands);
  player_object->Set(cur_hand_key, curr_hand);

  // return v8 array of hands
  args.GetReturnValue().Set(player_object);

}

void Game::GetDeckObject(const FunctionCallbackInfo<Value>& args) 
{
  // 
  Isolate* isolate = args.GetIsolate();
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());

  // get a copy of the deck
  auto d = obj->deck->deck_map;
  auto d_name = obj->deck->deck_map_name;
  // declare new v8 deck array
  v8::Handle<v8::Object> v8_deck = Object::New(isolate);

  int index = 0;
  // for each card name in the deck, get the card object
  for (auto it = d.begin(); it != d.end(); ++it)
  {
    Local<String> card_name = v8::String::NewFromUtf8(isolate, d_name[it->first].c_str());
    Local<Value> remaining = Number::New(isolate, it->second);

    // add the card object to the v8 object of cards
    v8_deck->Set(card_name, remaining);

    index++;
  }

  args.GetReturnValue().Set(v8_deck);
}

// NO LONGER NEEDED
void Game::GetCurrentHandValue(const v8::FunctionCallbackInfo<v8::Value>& args)
{
  Isolate* isolate = args.GetIsolate();
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());
  int val = obj->player->getCurrentHandValue();
  args.GetReturnValue().Set(Number::New(isolate, val));
}

void Game::GetDealerHandObject(const v8::FunctionCallbackInfo<v8::Value>& args)
{
  Isolate* isolate = args.GetIsolate();
  Game* obj = ObjectWrap::Unwrap<Game>(args.Holder());

  // declare new v8 object 
  Local<Object> dealer_hand_object = Object::New(isolate);

  Local<Value> value_key = v8::String::NewFromUtf8(isolate, "card_value");
  Local<Value> value = Number::New(isolate, obj->dealer_hand->total_val);

  string card_name = "";
  if (obj->dealer_hand->cards.size() > 0)
  {
    card_name = obj->dealer_hand->cards[0]->card_name;
  }
  Local<Value> card_key = v8::String::NewFromUtf8(isolate, "card_name");
  Local<Value> card = v8::String::NewFromUtf8(isolate, card_name.c_str());

  
  dealer_hand_object->Set(value_key, value);
  dealer_hand_object->Set(card_key, card);

  args.GetReturnValue().Set(dealer_hand_object);
}


void Game::PrintCurrentHand(Game *obj)
{
  obj->player->printCurrentHand();
}

void Game::PrintPlayerHands(Game *obj)
{
  obj->player->printHands();
}

void Game::PrintHandsVector(vector<Hand*> hands)
{
  cout << "PRINTING HANDS\n";
  for (unsigned int i = 0; i < hands.size(); ++i)
  {
    cout << "Hand #" << i << "\n"; 
    for (unsigned int j = 0; j < hands[i]->cards.size(); ++j)
    {
      cout << hands[i]->cards[j]->card_name << "\n";
    }
  }
}

void Game::PrintCardsVector(vector<Card*> cards)
{
  cout << "PRINTING CARDS\n";
  for (unsigned int i = 0; i < cards.size(); ++i)
  {
    cout << cards[i]->card_name << "\n";
  }
}


// }  // namespace BJ