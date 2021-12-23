using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Models.Mobile.Requests
{
    public class SetProfileThermometerRequest : MobileBaseRequest
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int ProfileId { get; set; }

        public int? DayOfWeek { get; set; }
        public int? Hour { get; set; }
        public int? Minutes { get; set; }
    }
}
