using System.Web;
using System.Web.Mvc;

namespace CodePound.PunchOutCalculator.WebApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}