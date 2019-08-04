﻿using System;
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
            CreateButton("Add Dealer Card", 17, 10, 3, 10, plainButton, AddPlayerCard_Clicked);

            // add player cards
            int startCol = 0;
            foreach (Card card in game.player.hands[game.player.currentHandIndex].cards)
            {
                CreateLabel(card.value.ToString(), 1, startCol++, 1, 1, defaultLabel);
            }
            // add the total
            CreateLabel(game.player.hands[game.player.currentHandIndex].totalValue.ToString(), 1, (NUM_COLS / 2) - 1, 1, 1, defaultLabel);

            // add dealer cards
            startCol = NUM_COLS / 2;
            foreach (Card card in game.dealer.hands[game.player.currentHandIndex].cards)
            {
                CreateLabel(card.value.ToString(), 1, startCol++, 1, 1, defaultLabel);
            }
            // add the total
            CreateLabel(game.dealer.hands[game.player.currentHandIndex].totalValue.ToString(), 1, NUM_COLS - 1, 1, 1, defaultLabel);

            this.Content = controlGrid;

        }

        #endregion

        #region Event Handlers

        private void AddDealerCard_Clicked(object sender, EventArgs e)
        {
            game.AddDealerCard(CardName.TWO);
            Draw();
        }

        private void AddPlayerCard_Clicked(object sender, EventArgs e)
        {
            game.AddPlayerCard(CardName.TWO);
            Draw();
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
