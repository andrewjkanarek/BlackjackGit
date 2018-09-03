// blackjack.cpp
#include <node.h>
#include "myobject.h"
#include "blackjack/game.h"

// namespace BJ {
// namespace demo {

using v8::FunctionCallbackInfo;
using v8::Isolate;
using v8::Local;
using v8::Object;
using v8::String;
using v8::Value;

void Method(const FunctionCallbackInfo<Value>& args) {
  Isolate* isolate = args.GetIsolate();
  args.GetReturnValue().Set(String::NewFromUtf8(isolate, "world"));
}

void Add(const FunctionCallbackInfo<Value>& args) {
  Isolate* isolate = args.GetIsolate();
  double value = args[0]->NumberValue() + args[1]->NumberValue();
  Local<v8::Number> num = v8::Number::New(isolate, value);
  args.GetReturnValue().Set(num);
}


void init(Local<Object> exports) {
  NODE_SET_METHOD(exports, "hello", Method);
  NODE_SET_METHOD(exports, "add", Add);
  MyObject::Init(exports);
  Game::Init(exports); // initializes game object
  
}

NODE_MODULE(addon, init)

// }  // namespace BJ