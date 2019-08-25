using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Player : PlayerBase
    {
        public Player() : base()
        {
            hands = new List<Hand> { new Hand() };
        }

        public override void Split()
        {
            hands.Add(new Hand());
        }
    }
}
