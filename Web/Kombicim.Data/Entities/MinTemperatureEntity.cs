namespace Kombicim.Data.Entities
{
    public class MinTemperatureEntity : CreationTimestampedEntity
    {
        public double Value { get; set; }
        public int LocationId { get; set; }
        public int ProfileId { get; set; }

        public virtual LocationEntity Location { get; set; }
        public virtual ProfileEntity Profile { get; set; }
    }
}
