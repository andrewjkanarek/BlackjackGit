using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BlackjackLib;

using Xamarin.Forms;

namespace BlackjackApp
{
    public partial class GamePage : ContentPage, INotifyPropertyChanged
    {
        #region Constants

        private const int NUM_ROWS = 20;
        private const int NUM_COLS = 20;

        #endregion

        #region Private Members

        private Game game;
        private List<string> cards;

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

        #region Public Members

        public List<string> Cards { get { return cards;  } }

        public string ProbWinStick { get { return game.ProbCounter.beforeHitStats.win.ToString("F4"); } }
        public string ProbLoseStick { get { return game.ProbCounter.beforeHitStats.lose.ToString("F4"); } }
        public string ProbPushStick { get { return game.ProbCounter.beforeHitStats.push.ToString("F4"); } }
        public string ProbWinHit { get { return game.ProbCounter.afterHitStats.win.ToString("F4"); } }
        public string ProbLoseHit { get { return game.ProbCounter.afterHitStats.lose.ToString("F4"); } }
        public string ProbPushHit { get { return game.ProbCounter.afterHitStats.push.ToString("F4"); } }

        public string Decision { get { return String.Format("You should... {0}", game.ProbCounter.decision.ToString()); } }

        #endregion

        #region Contructor

        public GamePage(Game game)
        {
            this.game = game;
            InitializeComponent();
            playerCardPicker.Unfocus();
            dealerCardPicker.Unfocus();
            cards = cardOptions.Keys
                .Where(s => !String.IsNullOrWhiteSpace(s))
                .ToList();
            BindingContext = this;
        }

        #endregion

        #region Initialize


        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Draw();
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

            this.Content = controlGrid;

        }

        #endregion

        #region Property Helpers

        private void UpdateStatsProperties()
        {
            OnPropertyChanged("ProbWinStick");
            OnPropertyChanged("ProbLoseStick");
            OnPropertyChanged("ProbPushStick");
            OnPropertyChanged("ProbWinHit");
            OnPropertyChanged("ProbLoseHit");
            OnPropertyChanged("ProbPushHit");
            OnPropertyChanged("Decision");
        }
        #endregion

        #region Draw Helpers

        private void DrawPlayerCards()
        {
            playerCardGrid.Children.Clear();

            // only add one row for now - more rows when splitting is added
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // create a column for each card in the hand
            for (int i = 0; i < game.Player.CurrentHand.Cards.Count; ++i)
            {
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                CreateLabel(game.Player.CurrentHand.Cards[i].Value.ToString(), 0, i, 1, 1, defaultLabel);
            }

            //// add player cards
            //int startCol = 0;
            //foreach (Card card in game.Player.CurrentHand.Cards)
            //{
            //    CreateLabel(card.Value.ToString(), 1, startCol, 1, 2, defaultLabel);
            //    startCol += 2;
            //}
            //// add the total
            //CreateLabel(game.Player.CurrentHand.TotalValue.ToString(), 1, (NUM_COLS / 2) - 2, 1, 2, defaultLabel);
        }

        private void DrawDealerCards()
        {
            // add dealer cards
            int startCol = NUM_COLS / 2;
            foreach (Card card in game.Dealer.CurrentHand.Cards)
            {
                CreateLabel(card.Value.ToString(), 1, startCol, 1, 2, defaultLabel);
                startCol += 2;
            }

            // add the total
            CreateLabel(game.Dealer.CurrentHand.TotalValue.ToString(), 1, NUM_COLS - 1, 1, 2, defaultLabel);
        }

        private void DrawStats()
        {
            // update the stats
            // prob win before hit
            CreateLabel(game.ProbCounter.beforeHitStats.win.ToString(), 8, 6, 1, 5, defaultLabel);
            // prob lose before hit
            CreateLabel(game.ProbCounter.beforeHitStats.lose.ToString(), 9, 6, 1, 5, defaultLabel);
            // prob push before hit
            CreateLabel(game.ProbCounter.beforeHitStats.push.ToString(), 10, 6, 1, 5, defaultLabel);
            // prob win after hit
            CreateLabel(game.ProbCounter.afterHitStats.win.ToString(), 8, 12, 1, 5, defaultLabel);
            // prob lose after hit
            CreateLabel(game.ProbCounter.afterHitStats.lose.ToString(), 9, 12, 1, 5, defaultLabel);
            // prob push after hit
            CreateLabel(game.ProbCounter.afterHitStats.push.ToString(), 10, 12, 1, 5, defaultLabel);
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

        private void PlayerCardPicker_Selected(object sender, EventArgs e)
        {
            if (playerCardPicker.SelectedIndex > -1)
            {
                playerCardPicker.Unfocus();
                string cardStr = playerCardPicker.Items[playerCardPicker.SelectedIndex];
                CardName cardName = cardOptions[cardStr];
                game.AddPlayerCard(cardName);
                UpdateStatsProperties();
                DrawPlayerCards();
            }

            playerCardPicker.SelectedIndex = -1;
            playerCardPicker.Unfocus();
        }

        private void DealerCardPicker_Selected(object sender, EventArgs e)
        {
            
            if (dealerCardPicker.SelectedIndex > -1)
            {
                dealerCardPicker.Unfocus();
                string cardStr = dealerCardPicker.Items[dealerCardPicker.SelectedIndex];
                CardName cardName = cardOptions[cardStr];
                game.AddDealerCard(cardName);
                UpdateStatsProperties();
            }

            dealerCardPicker.SelectedIndex = -1;
            dealerCardPicker.Unfocus();
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

        #endregion
    }
}
