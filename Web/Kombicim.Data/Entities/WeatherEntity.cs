using System.ComponentModel.DataAnnotations.Schema;

namespace Kombicim.Data.Entities
{
    public class WeatherEntity : CreationTimestampedEntity
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int LocationId { get; set; }

        [NotMapped]
        public new bool Active { get; set; }

        public virtual LocationEntity Location { get; set; }
    }
}
