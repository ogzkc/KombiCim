using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Arduino
{
    public class PostDeviceRequest : BaseDeviceRequest
    {
        [Required]
        public string TypeName { get; set; }
    }
}
