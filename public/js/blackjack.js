var app = angular.module('blackjackApp', []);


app.controller('blackjackCtrl', function($scope) 
{

    console.log("entering blackjack controller");

    var init = function()
    {
        console.log("entering init function");
        var req = new XMLHttpRequest();
        getGame = function()
        {
            if (req.status == 200 && req.readyState == 4)
            {
                var game = JSON.parse(req.responseText);
                console.log("GAME");
                console.log(game);
                $scope.deck = game.deck;
                $scope.player = game.player;
                $scope.dealer_hand = game.dealer_hand;
                $scope.$apply();
            }           
        }
        req.onreadystatechange = getGame;
        req.open("POST", '/', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send(null);
        console.log("request sent");
    }

    $scope.addPlayerCard = function(card_name)
    {
        $('#playerModal').modal('hide');
        card = 
        {
            name: card_name
        };
        console.log("addPlayerCard function called with value " + card_name);
        var req = new XMLHttpRequest();  
        getGame = function()
        {
            if (req.status == 200 && req.readyState == 4)
            {
                var game = JSON.parse(req.responseText);
                console.log("GAME");
                console.log(game);
                $scope.deck = game.deck;
                $scope.player = game.player;
                $scope.dealer_hand = game.dealer_hand;
                $scope.$apply();
            }           
        }
        req.onreadystatechange = getGame;
        req.open("POST", '/addPlayerCard', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send(JSON.stringify(card));
    }

    $scope.removeDeckCard = function(card_name)
    {
        console.log("entering removeDeckCard function with card " + card_name);
        console.log("card name: " + card_name);
        card = 
        {
            name: card_name
        };

        // remove card from deck and return player and deck object
        var req = new XMLHttpRequest();
        removeCard = function() 
        {
            if (req.readyState == 4 && req.status == 200) 
            {
                console.log("removeCard request successful");
                var game = JSON.parse(req.responseText);
                $scope.player = game.player;
                $scope.deck = game.deck;
                $scope.dealer_hand = game.dealer_hand;
                $scope.$apply(); 
            }
            else
            {
                // console.log("something went wrong with addDealerCard. Status: " + req.status + ". readyState: " + req.readyState);
            }
        }
        req.onreadystatechange = removeCard;
        req.open("POST", '/removeDeckCard', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send(JSON.stringify(card));
    } // removeDeckCard scope function

    // add dealer card to dealer hand
    $scope.addDealerCard = function(card_name)
    {
        $('#dealerModal').modal('hide');
        card = 
        {
            name: card_name
        };
        console.log("addDealerCard function called with value " + card_name);
        var req = new XMLHttpRequest();
        getDealerHand = function() 
        {
            if (req.readyState == 4 && req.status == 404) 
            {
                console.log("status 404");
            }
            else if (req.readyState == 4 && req.status == 200) 
            {
                console.log("getPlayerValue request successful");
                var game = JSON.parse(req.responseText);
                $scope.player = game.player;
                $scope.deck = game.deck;
                $scope.dealer_hand = game.dealer_hand;
                $scope.$apply(); 
            }
            else
            {
                // console.log("something went wrong with addDealerCard: " + req.status);
            }
        }
        req.onreadystatechange = getDealerHand;
        req.open("POST", '/addDealerCard', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send(JSON.stringify(card));
    } // addDealerCard scope function

    $scope.resetGame = function()
    {
        var req = new XMLHttpRequest();
        resetGame = function() 
        {
            if (req.readyState == 4 && req.status == 404) 
            {
                console.log("status 404");
            }
            else if (req.readyState == 4 && req.status == 200) 
            {
                console.log("resetGame request successful");
                var game = JSON.parse(req.responseText);
                $scope.player = game.player;
                $scope.deck = game.deck;
                $scope.dealer_hand = game.dealer_hand;
                $scope.$apply(); 
            }
        }
        req.onreadystatechange = resetGame;
        req.open("POST", '/resetGame', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send();
    }

    $scope.split = function(hand_index)
    {
        console.log("entering split function with hand index: " + hand_index);
        hand = 
        {
            hand_index: hand_index
        };
        var req = new XMLHttpRequest();
        getGame = function() 
        {
            if (req.readyState == 4 && req.status == 404) 
            {
                console.log("status 404");
            }
            else if (req.readyState == 4 && req.status == 200) 
            {
                console.log("getGame request successful");
                var game = JSON.parse(req.responseText);
                $scope.player = game.player;
                $scope.deck = game.deck;
                $scope.dealer_hand = game.dealer_hand;
                $scope.$apply(); 
            }
        }
        req.onreadystatechange = getGame;
        req.open("POST", '/split', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        console.log("sending request");
        req.send(JSON.stringify(hand));      
    }

    init();
});