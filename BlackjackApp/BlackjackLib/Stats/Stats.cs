using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class Stats
    {
        public decimal win { get; set; }
        public decimal lose { get; set; }
        public decimal push { get; set; }

        public Stats()
        {
            win = 0;
            lose = 0;
            push = 0;
        }
    }
}
