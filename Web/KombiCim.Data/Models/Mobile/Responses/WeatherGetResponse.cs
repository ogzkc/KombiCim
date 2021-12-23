using Kombicim.Data.Models.Mobile.Dtos;

namespace Kombicim.Data.Models.Mobile.Responses
{
    public class WeatherGetResponse : MobileBaseResponse
    {
        public List<WeatherData> WeatherDataList { get; set; }
    }
}
