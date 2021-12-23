using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Models.Mobile.Requests
{
    public class SetStateRequest : MobileBaseRequest
    {
        [Required]
        public bool State { get; set; }
    }
}
