using System;
using Xamarin.Forms;

namespace BlackjackApp
{
    public static class Helpers
    {
        public static int GetThicknessTop()
        {
            if (Device.RuntimePlatform == Device.iOS) return 20;

            return 0;

        }
    }
}
