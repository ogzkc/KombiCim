using System.ComponentModel.DataAnnotations.Schema;

namespace Kombicim.Data.Entities
{
    public class CombiLogEntity : CreationTimestampedEntity
    {
        public string DeviceId { get; set; }
        public bool State { get; set; }


        [NotMapped]
        public new bool Active { get; set; }

        public virtual DeviceEntity Device { get; set; }
    }
}
