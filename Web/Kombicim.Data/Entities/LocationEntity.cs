namespace Kombicim.Data.Entities
{
    public class LocationEntity : CreationTimestampedEntity
    {
        public string Name { get; set; }
        public string DeviceId { get; set; }

        public virtual DeviceEntity Device { get; set; }
        public virtual List<MinTemperatureEntity> MinTemperatures { get; set; } = new List<MinTemperatureEntity>();
        public virtual List<WeatherEntity> Weathers { get; set; } = new List<WeatherEntity>();
    }
}
