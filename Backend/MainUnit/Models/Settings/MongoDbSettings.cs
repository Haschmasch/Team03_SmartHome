namespace MainUnit.Models.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string RoomCollectionName { get; set; } = null!;
        public string ThermostatCollectionName { get; set; } = null!;
    }
}
