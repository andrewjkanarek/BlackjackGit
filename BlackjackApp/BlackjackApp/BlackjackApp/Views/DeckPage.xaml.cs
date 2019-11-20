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
            set
            {
                if (!ValidateCardCount(CardName.TWO, value)) return;
                deck.UpdateCardCount(CardName.TWO, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumThrees
        {
            get { return deck.CardCountDict[CardName.THREE].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.THREE, value)) return;
                deck.UpdateCardCount(CardName.THREE, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumFours
        {
            get { return deck.CardCountDict[CardName.FOUR].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.FOUR, value)) return;
                deck.UpdateCardCount(CardName.FOUR, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumFives
        {
            get { return deck.CardCountDict[CardName.FIVE].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.FIVE, value)) return;
                deck.UpdateCardCount(CardName.FIVE, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumSixes
        {
            get { return deck.CardCountDict[CardName.SIX].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.SIX, value)) return;
                deck.UpdateCardCount(CardName.SIX, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumSevens
        {
            get { return deck.CardCountDict[CardName.SEVEN].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.SEVEN, value)) return;
                deck.UpdateCardCount(CardName.SEVEN, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumEights
        {
            get { return deck.CardCountDict[CardName.EIGHT].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.EIGHT, value)) return;
                deck.UpdateCardCount(CardName.EIGHT, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumNines
        {
            get { return deck.CardCountDict[CardName.NINE].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.NINE, value)) return;
                deck.UpdateCardCount(CardName.NINE, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumTens
        {
            get { return deck.CardCountDict[CardName.TEN].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.TEN, value)) return;
                deck.UpdateCardCount(CardName.TEN, Int32.Parse(value));
                OnPropertyChanged();
            }
        }
        public string NumAces
        {
            get { return deck.CardCountDict[CardName.ACE].ToString(); }
            set
            {
                if (!ValidateCardCount(CardName.ACE, value)) return;
                deck.UpdateCardCount(CardName.ACE, Int32.Parse(value));
                OnPropertyChanged();
            }
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

        private bool ValidateCardCount(CardName cardName, string newCountStr)
        {
            int newCount;
            if (!Int32.TryParse(newCountStr, out newCount))
            {
                DisplayAlert("NanCardCount", "The card count must be a number.", "OK");
                return false;
            }

            if (newCount < 0)
            {
                DisplayAlert("NegCardCount", "The card count cannot be negative.", "OK");
                return false;
            }

            if (cardName == CardName.ACE && (newCount > (deck.DeckCount * 16)) ||
                cardName != CardName.ACE && (newCount > (deck.DeckCount * 4)))
            {
                DisplayAlert("LargeCardCount", "The card count you entered is too large for this deck.", "OK");
                return false;
            }

            return true;
        }

        #endregion


    }
}
