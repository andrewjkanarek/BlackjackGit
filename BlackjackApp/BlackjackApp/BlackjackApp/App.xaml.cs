using System;
using BlackjackLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BlackjackApp
{
    public partial class App : Application
    {
        GameSettings gameSettings;
        Game game;

        public App()
        {
            InitializeComponent();

            gameSettings = new GameSettings();
            game = new Game(gameSettings);

            MainPage = new NavigationPage(new MainPage(game));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
