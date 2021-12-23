using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Entities
{
    public class SettingEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string DeviceId { get; set; }
        public virtual DeviceEntity Device { get; set; }
    }
}
