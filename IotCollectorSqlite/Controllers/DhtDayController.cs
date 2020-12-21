using Microsoft.AspNetCore.Mvc;
using static IotCollectorSqlite.Controllers.DhtController;

namespace IotCollectorSqlite.Controllers
{
    [Route("[controller]/[action]")]
    public class DhtDayController : Controller
    {
        [HttpGet]
        public ActionResult Get(string stationId, string dateString, LookBack lookBack)
        {
            ViewBag.DateString = dateString;
            ViewBag.StationId = stationId;
            ViewBag.LookBack = lookBack;

            return View();
        }
    }
}
