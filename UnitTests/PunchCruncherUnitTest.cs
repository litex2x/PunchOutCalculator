using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodePound.PunchOutCalculator.WebApplication.Models;

namespace CodePound.PunchOutCalculator.PunchOutCalculatorUnitTest
{
    [TestClass]
    public class PunchCruncherUnitTest
    {
        [TestMethod]
        public void PunchOutTest()
        {
            PunchCruncher cruncher = new PunchCruncher( 
                DateTime.Parse(string.Format("{0} 7:55 AM", DateTime.Now.ToShortDateString())),
                DateTime.Parse(string.Format("{0} 12:20 PM", DateTime.Now.ToShortDateString())),
                DateTime.Parse(string.Format("{0} 1:27 PM", DateTime.Now.ToShortDateString())),
                480, false);

            Assert.AreEqual(60, cruncher.AdjustedLunchDuration);
            Assert.AreEqual(67, cruncher.LunchDuration);
            Assert.AreEqual(DateTime.Parse(string.Format("{0} 1:15 PM", DateTime.Now.ToShortDateString())).ToString(), cruncher.AdjustedLunchIn.ToString());
            Assert.AreEqual(DateTime.Parse(string.Format("{0} 12:15 PM", DateTime.Now.ToShortDateString())).ToString(), cruncher.AdjustedLunchOut.ToString());
            Assert.AreEqual(DateTime.Parse(string.Format("{0} 8:00 AM", DateTime.Now.ToShortDateString())).ToString(), cruncher.AdjustedPunchIn.ToString());
            Assert.AreEqual(TimeSpan.FromMinutes(255), cruncher.CurrentTotal);
            Assert.AreEqual(TimeSpan.FromMinutes(225), cruncher.MissingTotal); 
            Assert.AreEqual(DateTime.Parse(string.Format("{0} 4:53 PM", DateTime.Now.ToShortDateString())).ToString(), cruncher.PunchOut.ToString());
        }
    }
}
