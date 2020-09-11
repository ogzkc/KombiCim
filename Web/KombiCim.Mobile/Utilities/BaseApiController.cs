using KombiCim.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace KombiCim.Utilities
{
    public class BaseApiController : ApiController
    {
        public User ApiUser { get; set; }
    }
}