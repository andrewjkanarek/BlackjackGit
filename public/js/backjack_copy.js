

var app = angular.module('blackjackApp', []);


app.controller('blackjackCtrl', function($scope) 
{

	console.log("entering blackjack controller");

    var init = function()
    {
        console.log("entering init function");
        var player_req = new XMLHttpRequest();
        getPlayer = function() 
        {
            console.log("getPlayer function called");
            if (player_req.readyState == 4 && player_req.status == 404) 
            {
                console.log("status 404");
            }
            else if (player_req.readyState == 4 && player_req.status == 200) 
            {
                console.log("getPlayer request successful");
                response = JSON.parse(player_req.responseText);
                console.log(response);
                var player = response;
                var dealer_req = new XMLHttpRequest();
                getDealer = function() 
                {
                    if (dealer_req.readyState == 4 && dealer_req.status == 404) 
                    {
                        console.log("status 404");
                    }
                    else if (dealer_req.readyState == 4 && dealer_req.status == 200) 
                    {
                        console.log("getDealer request successful");
                        response = JSON.parse(dealer_req.responseText);
                        var dealer_hand = response;

                        // get the deck object
                        var deck_req = new XMLHttpRequest();
                        getDeck = function()
                        {
                            if (deck_req.readyState == 4 && deck_req.status == 200)
                            {
                                console.log("getDeck POST successfull");
                                response = JSON.parse(deck_req.responseText);
                                var deck = response; 
                                console.log("DECK");
                                console.log(deck.TEN);
                                $scope.deck = deck;
                                $scope.player = player;
                                $scope.dealer_hand = dealer_hand;
                                $scope.$apply();
                            }
                            else
                            {
                                // console.log("something went wrong with getDeck: " + deck_req.status);
                            }
                        }
                        deck_req.onreadystatechange = getDeck;
                        deck_req.open("POST", '/getDeckObject', true);
                        deck_req.setRequestHeader(
                            "Content-type",
                            "application/json"
                        );
                        deck_req.send();
                        
                    }
                }
                dealer_req.onreadystatechange = getDealer;
                dealer_req.open("POST", '/getDealerHandObject', true);
                dealer_req.setRequestHeader(
                    "Content-type",
                    "application/json"
                );
                dealer_req.send();

            }
            else 
            {
                // console.log("something went wrong with addDealerCard. Status: " + player_req.status + ". readyState: " + player_req.readyState);
            }
        }
        player_req.onreadystatechange = getPlayer;
        player_req.open("POST", '/', true);
        player_req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        player_req.send();
    } // init function

    $scope.playerHit = function()
    {
        location.href = "/addPlayerCard";
    }

    $scope.playerStand = function()
    {
        console.log("playerStand function called");
        /* CHANGE ME: The decision should not be changed to "BUSTED" */
        $scope.player.hands[$scope.player.curr_hand].decision = "BUSTED";
    }

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
                // location.href = "/addPlayerCard";
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

    $scope.removeDeckCard = function(card_name)
    {
        console.log("entering removeDeckCard function");
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
                response = JSON.parse(req.responseText);
                console.log(response);
                var deck = response.deck;
                var player = response.player;
                $scope.player = player;
                $scope.deck = deck;
                $scope.$apply(); 
            }
            else
            {
                console.log("something went wrong with addDealerCard. Status: " + req.status + ". readyState: " + req.readyState);
            }
        }
        req.onreadystatechange = removeCard;
        req.open("POST", '/removeDeckCard', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send(JSON.stringify(card));
    }

        // $scope.addPlayerCard = function(card_name)
    // {
    //     card = 
    //     {
    //         name: card_name
    //     };
    //     console.log("addPlayerCard function called with value " + card_name);
    //     var req = new XMLHttpRequest();
    //     getPlayerValue = function() 
    //     {
    //         if (req.readyState == 4 && req.status == 404) 
    //         {
    //             console.log("status 404");
    //         }
    //         else if (req.readyState == 4 && req.status == 200) 
    //         {
    //             console.log("getPlayerValue request successful");
    //             response = JSON.parse(req.responseText);
    //             var player = response;
    //             $scope.player = player;
    //             $scope.$apply(); 
    //             var dealer_req = new XMLHttpRequest();
    //             getDealer = function() 
    //             {
    //                 if (dealer_req.readyState == 4 && dealer_req.status == 404) 
    //                 {
    //                     console.log("status 404");
    //                 }
    //                 else if (dealer_req.readyState == 4 && dealer_req.status == 200) 
    //                 {
    //                     console.log("getDealer request successful");
    //                     response = JSON.parse(dealer_req.responseText);
    //                     var dealer_hand = response;
    //                     $scope.dealer_hand = player;
    //                     $scope.$apply();
    //                 }
    //             }
                
    //             dealer_req.onreadystatechange = getDealer;
    //             dealer_req.open("POST", '/getDealerHandObject', true);
    //             dealer_req.setRequestHeader(
    //                 "Content-type",
    //                 "application/json"
    //             );
    //             dealer_req.send();
    //         }
    //     }
    //     req.onreadystatechange = getPlayerValue;
    //     req.open("POST", '/addPlayerCard', true);
    //     req.setRequestHeader(
    //         "Content-type",
    //         "application/json"
    //     );
    //     req.send(JSON.stringify(card));
    // } // addPlayerCard scope function


    init();

});

app.controller('playerCardCtrl', function($scope) 
{

    console.log("entering card controller");

    $scope.message = "Pick Player Card";
    $scope.addPlayerCard = function(card_name)
    {
        card = 
        {
            name: card_name
        };
        console.log("addPlayerCard function called with value " + card_name);
        var req = new XMLHttpRequest();
        getPlayerValue = function() 
        {
            if (req.readyState == 4 && req.status == 404) 
            {
                console.log("status 404");
            }
            else if (req.readyState == 4 && req.status == 200) 
            {
                console.log("getPlayerValue request successful");
                response = JSON.parse(req.responseText);
                var player = response;
                console.log(player.hands[player.curr_hand].cards.length);
                var num_cards = player.hands[player.curr_hand].cards.length;
                if (num_cards == 0)
                {
                    console.log("HERE");
                    $scope.message = "Pick 1st Player Card";
                    $scope.$apply();
                }
                else if (num_cards == 1)
                {
                    $scope.message = "Pick 2nd Player Card";
                    $scope.$apply();
                }
                else if (num_cards >= 2)
                { 
                    var dealer_req = new XMLHttpRequest();
                    getDealer = function() 
                    {
                        if (dealer_req.readyState == 4 && dealer_req.status == 404) 
                        {
                            console.log("status 404");
                        }
                        else if (dealer_req.readyState == 4 && dealer_req.status == 200) 
                        {
                            console.log("getDealer request successful");
                            response = JSON.parse(dealer_req.responseText);
                            var dealer_hand = response;
                            if (dealer_hand.card_value == 0)
                            {
                                location.href = "/addDealerCard";
                            }
                            else
                            {
                                location.href = "/";
                            }
                        }
                    }
                    dealer_req.onreadystatechange = getDealer;
                    dealer_req.open("POST", '/getDealerHandObject', true);
                    dealer_req.setRequestHeader(
                        "Content-type",
                        "application/json"
                    );
                    dealer_req.send();
                }
            }
        }
        req.onreadystatechange = getPlayerValue;
        req.open("POST", '/addPlayerCard', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send(JSON.stringify(card));
    }


});

app.controller('dealerCardCtrl', function($scope) 
{
    // add dealer card to dealer hand
    $scope.addDealerCard = function(card_name)
    {
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
                location.href = "/";
            }
            else
            {
                console.log("something went wrong with addDealerCard: " + req.status);
            }
        }
        req.onreadystatechange = getDealerHand;
        req.open("POST", '/addDealerCard', true);
        req.setRequestHeader(
            "Content-type",
            "application/json"
        );
        req.send(JSON.stringify(card));
    }
});

// adds card to dealer's hand
// returns nothing
var reqAddDealerCard = function(card_name)
{
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
        }
    }
    req.onreadystatechange = getDealerHand;
    req.open("POST", '/addDealerCard', true);
    req.setRequestHeader(
        "Content-type",
        "application/json"
    );
    req.send(JSON.stringify(card));
};


// adds card to player's hand 
// returns player object
var reqAddPlayerCard = function(card_name)
{

    card = {
        name: card_name
    };
    console.log("addPlayerCard function called with value " + card_name);
    var req = new XMLHttpRequest();
    getPlayerValue = function() 
    {
        if (req.readyState == 4 && req.status == 404) 
        {
            console.log("status 404");
        }
        else if (req.readyState == 4 && req.status == 200) 
        {
            console.log("getPlayerValue request successful");
            response = JSON.parse(req.responseText);
            return response;
        }
    }
    req.onreadystatechange = getPlayerValue;
    req.open("POST", '/addPlayerCard', true);
    req.setRequestHeader(
        "Content-type",
        "application/json"
    );
    req.send(JSON.stringify(card));
}

// no input
// returns player object
var reqGetPlayerObject = function()
{
    var player_req = new XMLHttpRequest();
    getPlayer = function(playerHands) 
    {
        if (player_req.readyState == 4 && player_req.status == 404) 
        {
            console.log("status 404");
        }
        else if (player_req.readyState == 4 && player_req.status == 200) 
        {
            console.log("getPlayer request successful");
            response = JSON.parse(player_req.responseText);
            console.log(response);
            return response;
        }
    }
    player_req.onreadystatechange = getPlayer;
    player_req.open("POST", '/', true);
    player_req.setRequestHeader(
        "Content-type",
        "application/json"
    );
    player_req.send();
}

var reqGetDealerHandObject = function()
{
    var dealer_req = new XMLHttpRequest();
    getDealer = function() 
    {
        if (dealer_req.readyState == 4 && dealer_req.status == 404) 
        {
            console.log("status 404");
        }
        else if (dealer_req.readyState == 4 && dealer_req.status == 200) 
        {
            console.log("getDealer request successful");
            response = JSON.parse(dealer_req.responseText);
            return response;
        }
    }
    dealer_req.onreadystatechange = getDealer;
    dealer_req.open("POST", '/getDealerHandObject', true);
    dealer_req.setRequestHeader(
        "Content-type",
        "application/json"
    );
    dealer_req.send();
}


