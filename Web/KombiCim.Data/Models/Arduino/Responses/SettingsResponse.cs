using Kombicim.Data.Models.Arduino.Dtos;

namespace Kombicim.Data.Models.Arduino.Responses
{
    public class SettingsResponse : BaseDeviceResponse
    {
        public SettingsDto Settings { get; set; }
    }
}
