using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Dealer : PlayerBase
    {
        public Dealer() : base()
        {
            hands = new List<HandBase> { new HandDealer() };
        }

        public override void Split()
        {
            hands.Add(new HandDealer());
        }
    }
}
