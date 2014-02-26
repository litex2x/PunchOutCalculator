using Newtonsoft.Json;
using CodePound.PunchOutCalculator.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodePound.PunchOutCalculator.WebApplication.Controllers
{
    public class CalculationController : ApiController
    {   
        public string Post(PunchRequest request)
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

                return cruncher.PunchOut.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
