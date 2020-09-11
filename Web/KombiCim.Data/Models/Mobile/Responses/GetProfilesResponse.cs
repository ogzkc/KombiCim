﻿using KombiCim.Data.Models.Arduino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Mobile.Responses
{
    public class GetProfilesResponse : MobileBaseResponse
    {
        public List<ProfileDto> ProfileDtos { get; set; }
    }
}
