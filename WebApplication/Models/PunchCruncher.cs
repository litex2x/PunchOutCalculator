using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodePound.PunchOutCalculator.WebApplication.Models
{
    public class PunchCruncher
    {
        public DateTime PunchIn { get; private set; }
        public DateTime LunchOut { get; private set; }
        public DateTime LunchIn { get; private set; }
        public DateTime PunchOut
        {
            get
            {
                return AdjustedLunchIn.Add(MissingTotal - TimeSpan.FromMinutes(7));
            }
        }
        public DateTime AdjustedPunchIn
        {
            get
            {
                return PunchIn.AddMinutes(GetMinuteAdjustment(PunchIn.Minute));
            }
        }
        public DateTime AdjustedLunchOut
        {
            get
            {
                return LunchOut.AddMinutes(GetMinuteAdjustment(LunchOut.Minute));
            }
        }
        public DateTime AdjustedLunchIn
        {
            get
            {
                return AdjustedLunchOut.AddMinutes(AdjustedLunchDuration);
            }
        }
        public TimeSpan CurrentTotal
        {
            get
            {
                return AdjustedLunchOut - AdjustedPunchIn;
            }
        }
        public TimeSpan MissingTotal
        {
            get
            {
                return TimeSpan.FromMinutes(TargetTotalMinutes) - CurrentTotal;
            }
        }
        public int LunchDuration
        {
            get
            {
                TimeSpan actualLunch = LunchIn - LunchOut;

                return (int)actualLunch.TotalMinutes;
            }
        }
        public int AdjustedLunchDuration
        {
            get
            {
                return LunchDuration + GetMinuteAdjustment(LunchDuration);
            }
        }
        public int TargetTotalMinutes { get; set; }

        public PunchCruncher(DateTime punchIn, DateTime lunchOut, DateTime lunchIn, int targetTotalMinutes)
        {
            PunchIn = punchIn;
            LunchOut = lunchOut;
            LunchIn = lunchIn;
            TargetTotalMinutes = targetTotalMinutes;

            if (LunchOut <= PunchIn)
            {
                throw new ApplicationException("Punch in cannot be greater than or equal to lunch punch out");
            }
            else if (LunchIn <= LunchOut)
            {
                throw new ApplicationException("Lunch punch in cannot be less than or equal to lunch punch out");
            }
            else if (CurrentTotal >= TimeSpan.FromHours(8))
            {
                throw new ApplicationException("Already have 8 hours.");
            }
            else if (TargetTotalMinutes > 780)
            {
                throw new ApplicationException("Target total hours must be less than 13 hours.");
            }
        }

        private int GetMinuteAdjustment(int minute)
        {
            if ((minute % 15) == 0)
            {
                return 0;
            }
            else if ((minute % 15) > 7)
            {
                return 15 - (minute % 15);
            }
            else
            {
                return -1 * (minute % 15);
            }
        }
    }
}