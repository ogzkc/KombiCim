using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Models.Mobile.Requests
{
    public class PostProfileMinTemperatureRequest : MobileBaseRequest
    {
        [Required]
        public double Temperature { get; set; }

        [Required]
        public int ProfileId { get; set; }

        public int? DayOfWeek { get; set; }
        public int? Hour { get; set; }
        public int? Minutes { get; set; }
    }
}
