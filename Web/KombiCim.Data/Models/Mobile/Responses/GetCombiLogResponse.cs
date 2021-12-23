using Kombicim.Data.Models.Mobile.Dtos;

namespace Kombicim.Data.Models.Mobile.Responses
{
    public class GetCombiLogResponse : MobileBaseResponse
    {
        public double WorkedHours { get; set; }
        public List<CombiLogDto> CombiLogs { get; set; }
    }
}
