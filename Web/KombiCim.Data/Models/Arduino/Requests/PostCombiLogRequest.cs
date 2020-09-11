using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
{
    public class PostCombiLogRequest : BaseDeviceRequest
    {
        public bool State { get; set; }
    }
}
