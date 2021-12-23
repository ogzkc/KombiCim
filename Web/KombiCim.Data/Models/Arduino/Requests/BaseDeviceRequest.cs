using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Models.Arduino.Requests
{
    public class BaseDeviceRequest
    {
        [Required]
        [StringLength(8)]
        public string DeviceId { get; set; }
    }
}
