// initialize c++ addon
const addon = require('./build/Release/addon');
game = new addon.Game(5);

var express = require("express");
var app = express()
var path = require('path');
var http = require('http');
var bodyParser = require('body-parser');
var querystring = require('querystring');

// Create application/x-www-form-urlencoded parser
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// app.use(express.static('public'));
app.use(express.static(path.join(__dirname, 'public')));


app.get('/', function (req, res) {
	console.log("Got a GET request for the homepage");
   // res.redirect('/addPlayerCard');
   res.sendFile( __dirname + "/views/" + "index.html" );
})

// no longer used
// app.get('/addPlayerCard', function (req, res) {
//    console.log("Got a GET request for the homepage to addPlayerCard");
//    res.sendFile( __dirname + "/views/" + "addPlayerCard.html" );
// })

// no longer used
// app.get('/addDealerCard', function (req, res) {
//    console.log("Got a GET request for the homepage to addDealerCard");
//    res.sendFile( __dirname + "/views/" + "addDealerCard.html" );
// })

// app.post('/', function (req, res) {
//    console.log("homepage POST received");
//    res.set({ 'content-type': 'application/json; charset=utf-8' });
//    var player = game.getPlayerObject();
//    player = game.getPlayerObject();
//    console.log(player);
//    res.status(200).json(player);
// })
app.post('/', function (req, res) {
   console.log("homepage POST received");
   const deck = game.getDeckObject();
   const player = game.getPlayerObject();
   const dealer_hand = game.getDealerHandObject();
   const response = 
   {
      'deck': deck,
      'player': player,
      'dealer_hand': dealer_hand
   }
   res.status(200).json(response);
})



app.post('/addPlayerCard', function (req, res) {
   console.log("addPlayerCard POST received");
   const card_name = req.body.name;
   game.addPlayerCard(card_name);
   var hand_index = 0; // update this to be input from front end req
   game.updatePlayerStats(hand_index);
   const deck = game.getDeckObject();
   const player = game.getPlayerObject();
   const dealer_hand = game.getDealerHandObject();
   const response = 
   {
      'deck': deck,
      'player': player,
      'dealer_hand': dealer_hand
   }
   res.set({ 'content-type': 'application/json; charset=utf-8' });
   res.status(200).json(response);
})

app.post('/addDealerCard', function (req, res) {
   console.log("addDealerCard POST received");
   const card_name = req.body.name;
   game.addDealerCard(card_name);
   var hand_index = 0; // update this to be input from front end req
   game.updatePlayerStats(hand_index);
   const deck = game.getDeckObject();
   const player = game.getPlayerObject();
   const dealer_hand = game.getDealerHandObject();
   const response = 
   {
      'deck': deck,
      'player': player,
      'dealer_hand': dealer_hand
   }
   res.set({ 'content-type': 'application/json; charset=utf-8' });
   res.status(200).json(response);
})

// app.post('/getDealerHandObject', function (req, res) {
//    console.log("getDealerHandObject POST received");
//    res.set({ 'content-type': 'application/json; charset=utf-8' });
//    var dealer_hand = game.getDealerHandObject();
//    res.status(200).json(dealer_hand);
// })

// app.post('/getDeckObject', function (req, res) {
//    console.log("getDeckObject POST received");
//    const deck = game.getDeckObject();
//    // console.log(deck);
//    res.set({ 'content-type': 'application/json; charset=utf-8' });
//    res.status(200).json(deck);
// })

app.post('/removeDeckCard', function (req, res) {
   console.log("removeDeckCard POST received");
   const card_name = req.body.name;
   game.removeCard(card_name);
   var hand_index = 0; // update this to be input from front end req
   game.updatePlayerStats(hand_index);
   const deck = game.getDeckObject();
   const player = game.getPlayerObject();
   const dealer_hand = game.getDealerHandObject();
   const response = 
   {
      'deck': deck,
      'player': player,
      'dealer_hand': dealer_hand
   }
   res.status(200).json(response);
})

app.post('/resetGame', function (req, res) {
   console.log("resetGame POST received");
   game = new addon.Game();
   const deck = game.getDeckObject();
   const player = game.getPlayerObject();
   const dealer_hand = game.getDealerHandObject();
   const response = 
   {
      'deck': deck,
      'player': player,
      'dealer_hand': dealer_hand
   }
   res.set({ 'content-type': 'application/json; charset=utf-8' });
   res.status(200).json(response);
})

app.post('/split', function (req, res) {
   console.log("split POST received");
   const index = req.body.hand_index;
   console.log("hand index: " + index);
   const deck = game.getDeckObject();
   const player = game.getPlayerObject();
   const dealer_hand = game.getDealerHandObject();
   const response = 
   {
      'deck': deck,
      'player': player,
      'dealer_hand': dealer_hand
   }
   res.set({ 'content-type': 'application/json; charset=utf-8' });
   res.status(200).json(response);
})




var server = app.listen(8080, function () {
   var host = server.address().address
   var port = server.address().port
   
   console.log("Example app listening at http://%s:%s", host, port)
})






// hello.js

// console.log(addon.hello());

const obj = new addon.MyObject(10);



// card1 = "A";
// card2 = "A";
// card3 = "4";

// game.addDealerCard("10");
// game.addDealerCard("10");
// game.addDealerCard("2");

// game.addPlayerCard("10");
// game.addPlayerCard("10");
// game.addPlayerCard("2");
// game.addPlayerCard("4");
// game.addPlayerCard("2");
// game.addPlayerCard("10");
// game.addPlayerCard(card2);
// game.addPlayerCard(card3);

// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");
// game.removeCard("10");

// console.log(game.getCurrentHandValue());

// console.log(game.getCurrentHandCards());

console.log(game.getPlayerObject());
// console.log(game.getDealerHandObject());
// console.log(game.getDeckObject());


// game.AddCard(4);
// game.getCurrentHandTotal();







