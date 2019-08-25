using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Dealer : PlayerBase
    {
        public Dealer() : base()
        {
            hands = new List<Hand> { new Hand() };
        }

        public override void Split()
        {
            return;
        }
    }
}
