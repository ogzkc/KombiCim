using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
{
    public class BaseDeviceRequest
    {
        [Required]
        [StringLength(8)]
        public string DeviceId { get; set; }
    }
}
