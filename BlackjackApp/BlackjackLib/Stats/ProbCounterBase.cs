using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public abstract class ProbCounterBase
    {
        public Stats beforeHitStats { get; set; }
        public Stats afterHitStats { get; set; }

        protected ProbCounterBase()
        {
            beforeHitStats = new Stats();
            afterHitStats = new Stats();
        }

        public abstract void UpdateStats(HandBase hand);

    }
}
