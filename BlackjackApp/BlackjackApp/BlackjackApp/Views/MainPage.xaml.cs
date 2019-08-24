using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BlackjackLib;

namespace BlackjackApp
{
    public partial class MainPage : ContentPage
    {
        #region Constants

        private const int NUM_ROWS = 20;
        private const int NUM_COLS = 20;

        #endregion

        #region Private Members

        private Game game;
        private GameSettings gameSettings;
        private Picker playerCardPicker;
        private Picker dealerCardPicker;

        Dictionary<string, CardName> cardOptions = new Dictionary<string, CardName>
        {
            { "", CardName.NONE },
            { "2", CardName.TWO },         
            { "3", CardName.THREE },       
            { "4", CardName.FOUR },        
            { "5", CardName.FIVE },        
            { "6", CardName.SIX },
            { "7", CardName.SEVEN },
            { "8", CardName.EIGHT },
            { "9", CardName.NINE },
            { "10", CardName.TEN },
            { "ACE", CardName.ACE }
        };

        #endregion

        #region Contructor

        public MainPage()
        {
            InitializeComponent();
            Init();

            addDealerCardButton.Clicked += AddDealerCard_Clicked;
            addPlayerCardButton.Clicked += AddPlayerCard_Clicked;
        }

        #endregion

        #region Initialize

        public void Init()
        {
            gameSettings = new GameSettings
            {
                NumDecks = 4
            };

            game = new Game(gameSettings);

            CreateDealerCardPicker();
            CreatePlayerCardPicker();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Draw();
        }

        private void Draw()
        {

            // clear view
            controlGrid.Children.Clear();

            controlGrid = new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 1
            };

            // create rows def
            for (int i = 0; i < NUM_ROWS; ++i)
            {
                controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // create columns def
            for (int i = 0; i < NUM_COLS; ++i)
            {
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            //create cards row
            CreateLabel("Player", 0, 0, 5, 10, defaultLabel);
            CreateLabel("Dealer", 0, 10, 5, 10, defaultLabel);

            // create decision row
            CreateLabel("You should... HIT", 5, 0, 2, 20, defaultLabel);

            // Stats grid row
            CreateLabel("Stick", 7, 6, 1, 6, defaultLabel);
            CreateLabel("Hit", 7, 12, 1, 6, defaultLabel);
            CreateLabel("Win", 8, 0, 1, 6, defaultLabel);
            CreateLabel("Lose", 9, 0, 1, 6, defaultLabel);
            CreateLabel("Push", 10, 0, 1, 6, defaultLabel);

            // Button Actions Row
            CreateButton("Add Player Card", 17, 0, 3, 10, plainButton, AddPlayerCard_Clicked);
            CreateButton("Add Dealer Card", 17, 10, 3, 10, plainButton, AddDealerCard_Clicked);

            DrawPlayerCards();

            DrawDealerCards();

            DrawStats();

            // add Pickers to control
            controlGrid.Children.Add(playerCardPicker, 0, 0);
            controlGrid.Children.Add(dealerCardPicker, 0, 0);

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Helpers.GetThicknessTop(), 10, 5);

            this.Content = controlGrid;

        }

        #endregion

        #region Draw Helpers

        private void DrawPlayerCards()
        {
            // add player cards
            int startCol = 0;
            foreach (Card card in game.player.CurrentHand.cards)
            {
                CreateLabel(card.value.ToString(), 1, startCol, 1, 2, defaultLabel);
                startCol += 2;
            }
            // add the total
            CreateLabel(game.player.CurrentHand.totalValue.ToString(), 1, (NUM_COLS / 2) - 1, 1, 2, defaultLabel);
        }

        private void DrawDealerCards()
        {
            // add dealer cards
            int startCol = NUM_COLS / 2;
            foreach (Card card in game.dealer.CurrentHand.cards)
            {
                CreateLabel(card.value.ToString(), 1, startCol, 1, 2, defaultLabel);
                startCol += 2;
            }

            // add the total
            CreateLabel(game.dealer.CurrentHand.totalValue.ToString(), 1, NUM_COLS - 1, 1, 2, defaultLabel);
        }

        private void DrawStats()
        {
            // update the stats
            // prob win before hit
            CreateLabel(game.player.CurrentHand.probCounter.beforeHitStats.win.ToString(), 8, 6, 1, 2, defaultLabel);
            // prob lose before hit
            CreateLabel(game.player.CurrentHand.probCounter.beforeHitStats.lose.ToString(), 9, 6, 1, 2, defaultLabel);
            // prob push before hit
            CreateLabel(game.player.CurrentHand.probCounter.beforeHitStats.push.ToString(), 10, 6, 1, 2, defaultLabel);
            // prob win after hit
            CreateLabel(game.player.CurrentHand.probCounter.beforeHitStats.win.ToString(), 8, 12, 1, 2, defaultLabel);
            // prob lose after hit
            CreateLabel(game.player.CurrentHand.probCounter.beforeHitStats.win.ToString(), 9, 12, 1, 2, defaultLabel);
            // prob push after hit
            CreateLabel(game.player.CurrentHand.probCounter.beforeHitStats.win.ToString(), 10, 12, 1, 2, defaultLabel);
        }

        #endregion

        #region Event Handlers

        private void AddDealerCard_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => {
                dealerCardPicker.Focus();
            });
        }

        private void AddPlayerCard_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => {
                playerCardPicker.Focus();
            });
        }

        #endregion

        #region UI Helpers

        private void CreateLabel(string text, int col, int row, int rowSpan, int colSpan, Style style)
        {
            Label label = new Label { Text = text, Style = style };
            controlGrid.Children.Add(label, row, col);
            Grid.SetRowSpan(label, rowSpan);
            Grid.SetColumnSpan(label, colSpan);
        }

        private void CreateButton(string text, int col, int row, int rowSpan, int colSpan, Style style, EventHandler eventHandler)
        {
            Button button = new Button { Text = text, Style = style };
            controlGrid.Children.Add(button, row, col);
            Grid.SetRowSpan(button, rowSpan);
            Grid.SetColumnSpan(button, colSpan);
            button.Clicked += eventHandler;
        }

        private void CreatePlayerCardPicker()
        {
            CreateCardPicker(ref playerCardPicker, "Add Your Card");

            playerCardPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (playerCardPicker.SelectedIndex > -1)
                {
                    string cardStr = playerCardPicker.Items[playerCardPicker.SelectedIndex];
                    CardName cardName = cardOptions[cardStr];
                    game.AddPlayerCard(cardName);
                    Draw();

                }

                playerCardPicker.SelectedIndex = -1;
                playerCardPicker.Unfocus();

            };
        }

        private void CreateDealerCardPicker()
        {
            CreateCardPicker(ref dealerCardPicker, "Add Your Card");

            dealerCardPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (dealerCardPicker.SelectedIndex > -1)
                {
                    string cardStr = dealerCardPicker.Items[dealerCardPicker.SelectedIndex];
                    CardName cardName = cardOptions[cardStr];
                    game.AddDealerCard(cardName);

                    Draw();
                }

                playerCardPicker.SelectedIndex = -1;
                dealerCardPicker.Unfocus();

                

            };
        }

        private void CreateCardPicker(ref Picker picker, string text)
        {
            picker = new Picker
            {
                Title = text,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            foreach (string cardName in cardOptions.Keys)
            {
                picker.Items.Add(cardName);
            }

            picker.IsVisible = false;
            picker.IsEnabled = true;

        }

        #endregion

    }
}
