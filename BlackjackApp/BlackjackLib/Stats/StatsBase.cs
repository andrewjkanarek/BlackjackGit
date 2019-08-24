using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public class StatsBase
    {
        public decimal win;
        public decimal lose;
        public decimal push;

        protected StatsBase()
        {
            win = 0;
            lose = 0;
            push = 0;
        }
    }
}
