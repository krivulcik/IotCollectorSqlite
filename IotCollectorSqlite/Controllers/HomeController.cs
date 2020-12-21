using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IotCollectorSqlite.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Data = GetLatest().ToList();
            return View();
        }
        public IEnumerable<Station> GetLatest()
        {

            using var con = new SQLiteConnection(@"URI=file:./test.db");
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT station, max(timestamp) FROM dht_data GROUP BY station;";
            cmd.Prepare();

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                yield return new Station
                {
                    Name = rdr.GetString(0),
                    Timestamp = rdr.GetInt64(1),
                };
            }

            con.Close();
        }
    }
    public class Station
    {
        public string Name;
        public long Timestamp;
    }

}
