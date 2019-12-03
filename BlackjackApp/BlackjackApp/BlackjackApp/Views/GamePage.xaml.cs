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
        private List<Image> playerCardImages;
        private List<Image> dealerCardImages;


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

        Dictionary<CardName, string> cardImageDict = new Dictionary<CardName, string>
        {
            { CardName.TWO, "BlackjackApp.Static.Images.2H.png" },
            { CardName.THREE, "BlackjackApp.Static.Images.3H.png" },
            { CardName.FOUR, "BlackjackApp.Static.Images.4H.png" },
            { CardName.FIVE, "BlackjackApp.Static.Images.5H.png" },
            { CardName.SIX, "BlackjackApp.Static.Images.6H.png" },
            { CardName.SEVEN, "BlackjackApp.Static.Images.7H.png" },
            { CardName.EIGHT, "BlackjackApp.Static.Images.8H.png" },
            { CardName.NINE, "BlackjackApp.Static.Images.9H.png" },
            { CardName.TEN, "BlackjackApp.Static.Images.10H.png" },
            { CardName.ACE, "BlackjackApp.Static.Images.AH.png" }
        };

        #endregion

        #region Public Members

        public List<string> Cards { get { return cards;  } }

        public string ProbWinStick { get { return game.ProbCounter.BeforeHitStats.win.ToString("F4"); } }
        public string ProbLoseStick { get { return game.ProbCounter.BeforeHitStats.lose.ToString("F4"); } }
        public string ProbPushStick { get { return game.ProbCounter.BeforeHitStats.push.ToString("F4"); } }
        public string ProbWinHit { get { return game.ProbCounter.AfterHitStats.win.ToString("F4"); } }
        public string ProbLoseHit { get { return game.ProbCounter.AfterHitStats.lose.ToString("F4"); } }
        public string ProbPushHit { get { return game.ProbCounter.AfterHitStats.push.ToString("F4"); } }

        public string Decision { get { return String.Format("You should... {0}", game.ProbCounter.Decision.ToString()); } }
        public string PlayerTotal { get { return game.Player.CurrentHand.TotalValue.ToString(); } }
        public string DealerTotal { get { return game.Dealer.CurrentHand.TotalValue.ToString(); } }

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

            playerCardImages = new List<Image>();
            dealerCardImages = new List<Image>();

            BindingContext = this;
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
            OnPropertyChanged("PlayerTotal");
            OnPropertyChanged("DealerTotal");
        }
        #endregion

        #region Draw Helpers

        private void DrawPlayerCards()
        {
            // remove previous cards
            foreach (var image in playerCardImages)
            {
                playerCardGrid.Children.Remove(image);
            }
            playerCardImages.Clear();

            // add all cards
            for (int i = 0; i < game.Player.CurrentHand.Cards.Count; ++i)
            {
                playerCardImages.Add(CreateCardImage(playerCardGrid, game.Player.CurrentHand.Cards[i].Name, 1, i, 2, 2));
            }

        }

        private void DrawDealerCards()
        {
            // remove previous cards
            foreach (var image in dealerCardImages)
            {
                dealerCardGrid.Children.Remove(image);
            }
            dealerCardImages.Clear();

            // add all cards
            for (int i = 0; i < game.Dealer.CurrentHand.Cards.Count; ++i)
            {
                dealerCardImages.Add(CreateCardImage(dealerCardGrid, game.Dealer.CurrentHand.Cards[i].Name, 1, i, 2, 2));
            }
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
            CardPickerHelper(playerCardPicker, game.Player, DrawPlayerCards);
        }

        private void DealerCardPicker_Selected(object sender, EventArgs e)
        {
            CardPickerHelper(dealerCardPicker, game.Dealer, DrawDealerCards);
        }

        private delegate void DrawCards();
        private void CardPickerHelper(Picker picker, PlayerBase player, DrawCards drawCards)
        {
            if (picker.SelectedIndex > -1)
            {
                string cardStr = picker.Items[picker.SelectedIndex];
                CardName cardName = cardOptions[cardStr];
                game.AddCard(cardName, player);
                UpdateStatsProperties();
                drawCards();

                // show stats grid if players have appropriate amount of cards
                if (game.Dealer.CurrentHand.Cards.Count >= 1 && game.Player.CurrentHand.Cards.Count >= 2)
                {
                    probTable.IsVisible = true;
                    decisionLabel.IsVisible = true;
                    addDealerCardLabel.IsVisible = false;
                    addPlayerCardLabel.IsVisible = false;

                    game.UpdateStats();
                }
                // dealer must have at least 1 card
                else if (game.Dealer.CurrentHand.Cards.Count < 1)
                {
                    probTable.IsVisible = false;
                    decisionLabel.IsVisible = false;
                    addDealerCardLabel.IsVisible = true;
                    addPlayerCardLabel.IsVisible = false;
                }
                // player must have at least 2 cards
                else if (game.Player.CurrentHand.Cards.Count < 2)
                {
                    probTable.IsVisible = false;
                    decisionLabel.IsVisible = false;
                    addDealerCardLabel.IsVisible = false;
                    addPlayerCardLabel.IsVisible = true;
                }
            }

            picker.SelectedIndex = -1;
            picker.Unfocus();
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

        private Image CreateCardImage(Grid grid, CardName cardName, int col, int row, int rowSpan, int colSpan)
        {
            Image image = new Image { Source = ImageSource.FromResource(cardImageDict[cardName]) };
            grid.Children.Add(image, row, col);
            Grid.SetRowSpan(image, rowSpan);
            Grid.SetColumnSpan(image, colSpan);

            return image;
        }

        #endregion
    }
}
