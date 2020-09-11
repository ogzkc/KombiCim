using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using KombiCim.Data.Models;
using KombiCim.Data.Models.Mobile.Requests;
using KombiCim.Data.Models.Mobile.Responses;
using KombiCim.Data.Repository;
using KombiCim.Data.Utilities;
using KombiCim.Filters;
using KombiCim.Utilities;

namespace KombiCim.Controllers
{
    [Authentication]
    [ModelValidation]
    [TokenValidation]
    [Authorize(Roles = Roles.ROLE_MOBILE_APP)]
    public class WeatherController : BaseApiController
    {
        public async Task<WeatherGetResponse> Get(int lastHours = 6)
        {
            using (var db = new KombiCimEntities())
            {
                //var weathers = await db.CombiLogs.OrderByDescending(x => x.Id).ToListAsync();
                //for (int i = 0; i < weathers.Count; i++)
                //{
                //    weathers[i].Date = DateTime.Now.AddMinutes(-2 * i);
                //}

                //await db.SaveChangesAsync();

                return new WeatherGetResponse()
                {
                    WeatherDataList = await WeatherRepository.GetAll(ApiUser.Id, ApiUser.OwnedDeviceId, lastHours: lastHours, db_: db)
                };

            }
        }
    }
}
