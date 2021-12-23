namespace Kombicim.Data.Models.Mobile.Requests
{
    public class WeatherGetRequest : MobileBaseRequest
    {
        public int LastHours { get; set; } = 6;
    }
}
