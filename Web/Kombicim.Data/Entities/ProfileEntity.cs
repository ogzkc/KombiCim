namespace Kombicim.Data.Entities
{
    public class ProfileEntity : BaseEntity
    {
        public string Name { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual List<MinTemperatureEntity> MinTemperatures { get; set; } = new List<MinTemperatureEntity>();
        public virtual List<ProfilePreferenceEntity> ProfilePreferences { get; set; } = new List<ProfilePreferenceEntity>();
    }
}
