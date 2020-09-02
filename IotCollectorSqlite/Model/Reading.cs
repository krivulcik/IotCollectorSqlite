namespace IotCollectorSqlite.Model
{
    public class Reading
    {
        public long Timestamp { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
    }
}
