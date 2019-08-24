using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Player : PlayerBase
    {
        public Player() : base()
        {
            hands = new List<HandBase> { new HandPlayer() };
        }

        public override void Split()
        {
            hands.Add(new HandPlayer());
        }
    }
}
