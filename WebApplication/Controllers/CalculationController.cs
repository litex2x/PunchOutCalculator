using CodePound.PunchOutCalculator.WebApplication.Models;
using System;
using System.Web.Http;
using CodePound.PunchOutCalculator.NovaLogic;

namespace CodePound.PunchOutCalculator.WebApplication.Controllers
{
    public class CalculationController : ApiController
    {
        public PunchResponse Post(PunchRequest request)
        {
            PunchCruncher cruncher = null;

            try
            {
                cruncher = new PunchCruncher(
                    DateTime.Parse(request.PunchIn),
                    DateTime.Parse(request.LunchOut),
                    DateTime.Parse(request.LunchIn),
                    Convert.ToInt32(request.TargetTotalMinutes),
                    Convert.ToBoolean(request.IsLunchOverrideEnabled));

                return new PunchResponse { Result = cruncher.GetPunchOut().ToString(), IsSuccessful = true };
            }
            catch (ArgumentException exception)
            {
                return new PunchResponse { Result = exception.Message, IsSuccessful = false };
            }
        }
    }
}
