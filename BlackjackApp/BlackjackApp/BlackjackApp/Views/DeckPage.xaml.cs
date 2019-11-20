using System;
using System.Collections.Generic;
using BlackjackLib;
using Xamarin.Forms;

namespace BlackjackApp
{
    public partial class DeckPage : ContentPage
    {
        #region Properties

        public string NumDecks
        {
            get
            {
                return deck.DeckCount.ToString();
            }
            set
            {
                // validate input is a number
                int numDecks;
                if (!Int32.TryParse(value, out numDecks)) return;

                // don't update deck if no change was made
                if (numDecks == deck.DeckCount) return;

                deck.UpdateDeckCount(numDecks);

                // update the properties so the UI binding reflects changes
                UpdateCardCountProperties();
            }
        }

        public string NumTwos
        {
            get { return deck.CardCountDict[CardName.TWO].ToString(); }
        }
        public string NumThrees
        {
            get { return deck.CardCountDict[CardName.THREE].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumFours
        {
            get { return deck.CardCountDict[CardName.FOUR].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumFives
        {
            get { return deck.CardCountDict[CardName.FIVE].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumSixes
        {
            get { return deck.CardCountDict[CardName.SIX].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumSevens
        {
            get { return deck.CardCountDict[CardName.SEVEN].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumEights
        {
            get { return deck.CardCountDict[CardName.EIGHT].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumNines
        {
            get { return deck.CardCountDict[CardName.NINE].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumTens
        {
            get { return deck.CardCountDict[CardName.TEN].ToString(); }
            set { OnPropertyChanged(); }
        }
        public string NumAces
        {
            get { return deck.CardCountDict[CardName.ACE].ToString(); }
            set { OnPropertyChanged(); }
        }

        #endregion

        private Deck deck;

        public DeckPage(Deck deck)
        {
            this.deck = deck;
            InitializeComponent();

            BindingContext = this;
        }

        #region Event Handlers

        public void DeckCount_Changed(object sender, TextChangedEventArgs e)
        {
            int numDecks;
            if (!Int32.TryParse(e.NewTextValue, out numDecks)) return;

            deck.UpdateDeckCount(numDecks);
        }

        #endregion

        #region Private Helpers

        private void UpdateCardCountProperties()
        {
            OnPropertyChanged("NumTwos");
            OnPropertyChanged("NumThrees");
            OnPropertyChanged("NumFours");
            OnPropertyChanged("NumFives");
            OnPropertyChanged("NumSixes");
            OnPropertyChanged("NumSevens");
            OnPropertyChanged("NumEights");
            OnPropertyChanged("NumNines");
            OnPropertyChanged("NumTens");
            OnPropertyChanged("NumAces");
        }

        #endregion


    }
}
