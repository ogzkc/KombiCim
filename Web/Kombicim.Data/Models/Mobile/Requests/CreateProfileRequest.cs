namespace Kombicim.Data.Models.Mobile.Requests
{
    public class CreateProfileRequest : MobileBaseRequest
    {
        public int ProfileTypeId { get; set; }
        public string Name { get; set; }
    }
}
