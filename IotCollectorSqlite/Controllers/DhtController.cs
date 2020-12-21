using IotCollectorSqlite.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;

namespace IotCollectorSqlite.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DhtController : ControllerBase
    {
        private IConfiguration _configuration;
        public DhtController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string Get(string i, string t = "0", string h = "0")
        {
            var temperature = double.Parse(t, CultureInfo.InvariantCulture);
            var humidity = int.Parse(h, CultureInfo.InvariantCulture);

            StoreReading(i, temperature, humidity);

            return "ok";
        }

        public void StoreReading(string stationId, double temperature, int humidity)
        {
            using var con = new SQLiteConnection(@"URI=file:" + _configuration["DatabasePath"]);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "INSERT INTO dht_data(timestamp, station, temperature, humidity) VALUES(strftime('%s','now'), @station, @temperature, @humidity)";

            cmd.Parameters.AddWithValue("@station", stationId);
            cmd.Parameters.AddWithValue("@temperature", temperature);
            cmd.Parameters.AddWithValue("@humidity", humidity);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            con.Close();
        }

        [HttpGet]
        public Dictionary<string, double[][]> GetDay(string stationId, string dateString, LookBack lookBack)
        {
            var date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var readings = GetReadings(stationId, date, lookBack).ToArray();

            return new Dictionary<string, double[][]>
            {
                { "Temperature", readings.Select(x => new double[] { x.Timestamp * 1000, x.Temperature }).ToArray() },
                { "Humidity", readings.Select(x => new double[] { x.Timestamp * 1000, x.Humidity }).ToArray() },
            };
        }

        public IEnumerable<Reading> GetReadings(string stationId, DateTime date, LookBack lookBack = LookBack.Day)
        {
            var startDate = date.AddDays(-(int)lookBack);

            using var con = new SQLiteConnection(@"URI=file:" + _configuration["DatabasePath"]);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT timestamp, temperature, humidity FROM dht_data WHERE station = @station AND timestamp >= @startTimestamp AND timestamp < @endTimestamp ORDER BY timestamp ASC LIMIT 30000;";

            cmd.Parameters.AddWithValue("@station", stationId);
            cmd.Parameters.AddWithValue("@startTimestamp", ((DateTimeOffset)startDate).ToUnixTimeSeconds());
            cmd.Parameters.AddWithValue("@endTimestamp", ((DateTimeOffset)date).ToUnixTimeSeconds());
            cmd.Prepare();

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                yield return new Reading
                {
                    Timestamp = rdr.GetInt64(0),
                    Temperature = rdr.GetDouble(1),
                    Humidity = rdr.GetInt32(2),
                };
            }

            con.Close();
        }

        public enum LookBack{
            Day = 1,
            Week = 7,
            Month = 30
        }
    }
}
