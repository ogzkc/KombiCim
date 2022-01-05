namespace Kombicim.Data.Entities
{
    public  class ProfilePreferenceEntity : CreationTimestampedEntity
    {
        //TODO: upcoming feature
        public int ProfileId { get; set; }
        public string DeviceId { get; set; }
        public int? ActiveProfileId { get; set; }

        public virtual ProfileEntity Profile { get; set; }
        public virtual DeviceEntity Device { get; set; }
    }
}
