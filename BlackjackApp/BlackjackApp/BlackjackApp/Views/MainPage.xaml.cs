using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BlackjackLib;

namespace BlackjackApp
{
    public partial class MainPage : MasterDetailPage
    {
        MasterPage masterPage;
        Game game;

        public MainPage(Game game)
        {
            this.game = game;

            masterPage = new MasterPage();
            Master = masterPage;
            Detail = new NavigationPage(new GamePage(this.game));

            masterPage.menuItemListView.ItemSelected += OnMenuItemSelected;

            //InitializeComponent();

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;

            if (item == null) return;

            masterPage.menuItemListView.SelectedItem = null;
            IsPresented = false;

            if (item.Identifier == "deckItem")
            {
                await Navigation.PushAsync(new DeckPage());
            }
            else if (item.Identifier == "rstGameItem")
            {
                game = new Game(game.GameSettings);
                Detail = new NavigationPage(new GamePage(game));
            }


        }


    }
}
