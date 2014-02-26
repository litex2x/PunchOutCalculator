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

        public int TargetTotalMinutes { get; private set; }

        public bool IsLunchOverrideEnabled { get; private set; }

        public DateTime PunchOut
        {
            get
            {
                if (MissingTotal > TimeSpan.FromHours(5) && !IsLunchOverrideEnabled)
                {
                    throw new ApplicationException("Target hourly total is too high.");
                }

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
                if (AdjustedLunchDuration < 30 && !IsLunchOverrideEnabled)
                {
                    throw new ApplicationException("Lunch punch in is too low");
                }

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
                if (CurrentTotal > TimeSpan.FromHours(5) && !IsLunchOverrideEnabled)
                {
                    throw new ApplicationException("Lunch punch out is too high");
                }

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

        public PunchCruncher(DateTime punchIn, DateTime lunchOut, DateTime lunchIn, int targetTotalMinutes, bool isLunchOverrideEnabled)
        {
            PunchIn = punchIn;
            LunchOut = lunchOut;
            LunchIn = lunchIn;
            TargetTotalMinutes = targetTotalMinutes;
            IsLunchOverrideEnabled = isLunchOverrideEnabled;

            if (LunchOut <= PunchIn)
            {
                throw new ApplicationException("Punch in cannot be greater than or equal to lunch punch out");
            }
            else if (LunchIn <= LunchOut)
            {
                throw new ApplicationException("Lunch punch in cannot be less than or equal to lunch punch out");
            }
            else if (CurrentTotal >= TimeSpan.FromMinutes(targetTotalMinutes))
            {
                throw new ApplicationException("Current hourly total is greater than or equal to target hourly total.");
            }
            else if (TargetTotalMinutes > 600 && !IsLunchOverrideEnabled)
            {
                throw new ApplicationException("Target hourly total must be less than 10 hours.");
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