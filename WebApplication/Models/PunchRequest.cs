using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodePound.PunchOutCalculator.WebApplication.Models
{
    public class PunchRequest
    {
        public string PunchIn { get; set; }
        public string LunchOut { get; set; }
        public string LunchIn { get; set; }
        public string TargetTotalMinutes { get; set; }
    }
}