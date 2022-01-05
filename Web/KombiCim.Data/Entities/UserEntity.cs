namespace Kombicim.Data.Entities
{
    public class UserEntity : BaseEntity
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }

        public virtual DeviceEntity Device { get; set; }
        public virtual List<ApiTokenEntity> ApiTokens { get; set; } = new List<ApiTokenEntity>();
        public virtual List<DeviceEntity> Devices { get; set; } = new List<DeviceEntity>();
        public virtual List<ProfileEntity> Profiles { get; set; } = new List<ProfileEntity>();
    }
}
