using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodePound.PunchOutCalculator.NovaLogic;

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

            Assert.AreEqual(DateTime.Parse(string.Format("{0} 4:53 PM", DateTime.Now.ToShortDateString())).ToString(), cruncher.GetPunchOut().ToString());
        }
    }
}
