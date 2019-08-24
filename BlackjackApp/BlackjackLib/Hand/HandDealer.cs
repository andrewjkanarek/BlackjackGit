using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class HandDealer : HandBase
    {
        public HandDealer() : base() { }


        // stats should not be updated for the dealer
        protected override void UpdateStats() { }

    }
}
