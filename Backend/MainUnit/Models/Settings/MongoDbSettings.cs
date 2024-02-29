namespace MainUnit.Models.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string RoomCollectionName { get; set; } = null!;
        //This should be a timeseries
        public string RoomTemperatureCollectionName { get; set; } = null!;
        public string ThermostatCollectionName { get; set; } = null!;
        public string AuthCollectionName { get; set; } = null!;
    }
}
