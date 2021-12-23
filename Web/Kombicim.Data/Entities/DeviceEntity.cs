using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kombicim.Data.Entities
{
    public class DeviceEntity
    {
        [Key]
        public string Id { get; set; }
        public string CenterDeviceId { get; set; }
        public int TypeId { get; set; } // 1 = center, 2 = thermometer
        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual List<CombiLogEntity> CombiLogs { get; set; } = new List<CombiLogEntity>();
        public virtual List<LocationEntity> Locations { get; set; } = new List<LocationEntity>();
        public virtual List<ProfilePreferenceEntity> ProfilePreferences { get; set; } = new List<ProfilePreferenceEntity>();
        public virtual List<SettingEntity> Settings { get; set; } = new List<SettingEntity>();
        public virtual List<CombiStateEntity> CombiStates { get; set; } = new List<CombiStateEntity>();
        public virtual List<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
