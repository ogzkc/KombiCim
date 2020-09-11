using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Models.Mobile.Requests
{
    public class SetStateRequest : MobileBaseRequest
    {
        [Required]
        public bool State { get; set; }
    }
}
