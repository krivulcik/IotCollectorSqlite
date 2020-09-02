using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IotCollectorSqlite.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IotCollectorSqlite.Controllers
{
    [Route("[controller]/[action]")]
    public class DhtDayController : Controller
    {
        [HttpGet]
        public ActionResult Get(string stationId, string dateString)
        {
            ViewBag.DateString = dateString;
            ViewBag.StationId = stationId;

            return View();
        }
    }
}
