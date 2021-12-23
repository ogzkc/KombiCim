using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Models.Arduino.Requests
{
    public class PostDeviceRequest : BaseDeviceRequest
    {
        [Required]
        public string TypeName { get; set; }
    }
}
