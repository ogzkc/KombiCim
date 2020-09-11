using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using KombiCim.Data.Models.Arduino.Responses;

namespace KombiCim.Arduino.Controllers
{
    [Authentication]
    [ModelValidation]
    public class DeviceController : ApiController
    {
        public async Task<PostDeviceResponse> Post([FromBody]PostDeviceRequest request)
        {
            using (var db = new KombiCimEntities())
            {
                await DeviceRepository.Post(request.DeviceId, request.TypeName, db_: db);
                return new PostDeviceResponse();
            }
        }
    }
}
