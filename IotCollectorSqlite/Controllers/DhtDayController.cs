using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IotCollectorSqlite.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
