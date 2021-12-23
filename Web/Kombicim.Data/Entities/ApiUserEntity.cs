namespace Kombicim.Data.Entities
{
    public class ApiUserEntity : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int AuthTypeId { get; set; } // 1 = IoTDevice, 2 = MobileApp
    }
}
