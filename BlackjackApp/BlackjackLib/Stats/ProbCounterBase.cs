using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackLib
{
    public abstract class ProbCounterBase
    {
        #region Private Members

        private Stats beforeHitStats;
        private Stats afterHitStats;

        private enum PossibleOutcome
        {
            SEVENTEEN,
            EIGHTEEN,
            NINETEEN,
            TWENTY,
            TWENTYONE,
            BUST,
            NOBUST
        }

        // key: outcome, probability of outcome
        private Dictionary<PossibleOutcome, decimal> playerProbDict = new Dictionary<PossibleOutcome, decimal>
        {
            { PossibleOutcome.SEVENTEEN, 0 },
            { PossibleOutcome.EIGHTEEN, 0 },
            { PossibleOutcome.NINETEEN, 0 },
            { PossibleOutcome.TWENTY, 0 },
            { PossibleOutcome.TWENTYONE, 0 },
            { PossibleOutcome.NOBUST, 0 }
        };

        // key: outcome, probability of outcome
        private Dictionary<PossibleOutcome, decimal> dealerProbDict = new Dictionary<PossibleOutcome, decimal>
        {
            { PossibleOutcome.SEVENTEEN, 0 },
            { PossibleOutcome.EIGHTEEN, 0 },
            { PossibleOutcome.NINETEEN, 0 },
            { PossibleOutcome.TWENTY, 0 },
            { PossibleOutcome.TWENTYONE, 0 },
            { PossibleOutcome.NOBUST, 0 }
        };

        #endregion

        #region Public Members

        public Stats BeforeHitStats { get { return beforeHitStats; } }
        public Stats AfterHitStats { get { return afterHitStats; } }

        #endregion

        protected ProbCounterBase()
        {
            beforeHitStats = new Stats();
            afterHitStats = new Stats();
        }

        #region Public Functions

        public abstract void UpdateStats(HandPlayer playerHand, HandDealer dealerHand, Deck deck);

        #endregion

        #region Private Functions

        protected void ResetStats()
        {
            // reset prob dicts
            ResetProbDict(playerProbDict);
            ResetProbDict(dealerProbDict);
            return;
        }

        protected void SetDealerProbMap()
        {
            //if ()
        }

        protected void SetPlayerProbMap()
        {

        }

        #endregion

        #region Helpers

        private void ResetProbDict(Dictionary<PossibleOutcome, decimal> probDict)
        {
            if (probDict == null || probDict.Keys.Count == 0) return;

            foreach (var key in probDict.Keys)
            {
                probDict[key] = 0;
            }
        }

        #endregion

    }
}
