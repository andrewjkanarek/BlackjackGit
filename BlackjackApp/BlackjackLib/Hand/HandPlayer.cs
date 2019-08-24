using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class HandPlayer : HandBase
    {
        public HandPlayer() : base() { }

        protected override void UpdateStats()
        {
            probCounter.UpdateStats(this);
        }
    }
}
