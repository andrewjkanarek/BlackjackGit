﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class ProbCounterSlow : ProbCounterBase
    {
        public ProbCounterSlow() : base() { }
        
        public override void UpdateStats(Hand playerHand, Hand dealerHand, Deck deck)
        {
            ResetStats();
            return;
        }
    }
}