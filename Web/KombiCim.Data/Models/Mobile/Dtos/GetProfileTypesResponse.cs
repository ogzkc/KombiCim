using Kombicim.Data.Models.Mobile.Dtos;

namespace Kombicim.Data.Models.Mobile.Responses
{
    public class GetProfileTypesResponse : MobileBaseResponse
    {
        public List<ProfileTypeDto> ProfileTypeDtos { get; set; }
    }
}
