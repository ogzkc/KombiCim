using System.ComponentModel.DataAnnotations.Schema;

namespace Kombicim.Data.Entities
{
    public class CombiStateEntity : CreationTimestampedEntity
    {
        public bool Value { get; set; }
        public string DeviceId { get; set; }

        [NotMapped]
        public new bool Active { get; set; }


        public virtual DeviceEntity Device { get; set; }
    }
}
