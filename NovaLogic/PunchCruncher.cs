using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodePound.PunchOutCalculator.NovaLogic
{
    public class PunchCruncher
    {
        public DateTime PunchIn { get; private set; }

        public DateTime LunchOut { get; private set; }

        public DateTime LunchIn { get; private set; }

        public int TargetTotalMinutes { get; private set; }

        public bool IsLunchOverrideEnabled { get; private set; }

        public PunchCruncher(DateTime punchIn, DateTime lunchOut, DateTime lunchIn, int targetTotalMinutes, bool isLunchOverrideEnabled)
        {
            PunchIn = punchIn;
            LunchOut = lunchOut;
            LunchIn = lunchIn;
            TargetTotalMinutes = targetTotalMinutes;
            IsLunchOverrideEnabled = isLunchOverrideEnabled;

            if (LunchOut <= PunchIn)
            {
                throw new ArgumentException("Punch in cannot be greater than or equal to lunch punch out");
            }
            else if (LunchIn <= LunchOut)
            {
                throw new ArgumentException("Lunch punch in cannot be less than or equal to lunch punch out");
            }
            else if (TargetTotalMinutes > 600 && !IsLunchOverrideEnabled)
            {
                throw new ArgumentException("Target hourly total must be less than 10 hours.");
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

        private TimeSpan GetMissingTotal(TimeSpan currentTotal)
        {
            if (currentTotal > TimeSpan.FromHours(5) && !IsLunchOverrideEnabled)
            {
                throw new ArgumentException("Lunch punch out is too high");
            }
            else
            {
                return TimeSpan.FromMinutes(TargetTotalMinutes) - currentTotal;
            }
        }

        private DateTime GetAdjustedLunchIn(DateTime adjustedLunchOut, double adjustedLunchDuration)
        {
            if (adjustedLunchDuration < 30 && !IsLunchOverrideEnabled)
            {
                throw new ArgumentException("Lunch punch in is too low");
            }
            else
            {
                return adjustedLunchOut.AddMinutes(adjustedLunchDuration);
            }
        }

        private DateTime GetPunchOut(DateTime adjustedLunchIn, TimeSpan missingTotal)
        {
            if (missingTotal > TimeSpan.FromHours(5) && !IsLunchOverrideEnabled)
            {
                throw new ArgumentException("Target hourly total is too high.");
            }
            else
            {
                return adjustedLunchIn.Add(missingTotal - TimeSpan.FromMinutes(7));
            }
        }

        private TimeSpan GetCurrentTotal(DateTime adjustedLunchOut, DateTime adjustedPunchIn)
        {
            TimeSpan currentTotal = adjustedLunchOut - adjustedPunchIn;

            if (currentTotal >= TimeSpan.FromMinutes(TargetTotalMinutes))
            {
                throw new ArgumentException("Current hourly total is greater than or equal to target hourly total.");
            }
            else
            {
                return currentTotal;
            }
        }

        public DateTime GetPunchOut()
        {
            DateTime adjustedPunchIn = PunchIn.AddMinutes(GetMinuteAdjustment(PunchIn.Minute));
            DateTime adjustedLunchOut = LunchOut.AddMinutes(GetMinuteAdjustment(LunchOut.Minute));
            TimeSpan currentTotal = GetCurrentTotal(adjustedLunchOut, adjustedPunchIn);
            double lunchDuration = (LunchIn - LunchOut).TotalMinutes;
            double adjustedLunchDuration = lunchDuration + GetMinuteAdjustment((int)lunchDuration);
            TimeSpan missingTotal = GetMissingTotal(currentTotal);
            DateTime adjustedLunchIn = GetAdjustedLunchIn(adjustedLunchOut, adjustedLunchDuration);
            DateTime punchOut = GetPunchOut(adjustedLunchIn, missingTotal);

            return punchOut;
        }
    }
}