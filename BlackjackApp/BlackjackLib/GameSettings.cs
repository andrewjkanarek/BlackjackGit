using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class GameSettings
    {
        public int NumDecks { get; set; }

        public GameSettings()
        {
            NumDecks = 4;
        }

        public GameSettings(int numDecks)
        {
            NumDecks = numDecks;
        }
    }
}
