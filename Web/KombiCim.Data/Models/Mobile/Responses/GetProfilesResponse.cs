﻿using Kombicim.Data.Models.Arduino.Dtos;

namespace Kombicim.Data.Models.Mobile.Responses
{
    public class GetProfilesResponse : MobileBaseResponse
    {
        public List<ProfileDto> ProfileDtos { get; set; }
    }
}
