using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KombiCim.Data.Models.Mobile.Dtos;
using System.Data.Entity;

namespace KombiCim.Data.Repository
{
    public class CombiLogRepository : BaseRepository
    {
        public static async Task Post(string deviceId, bool state, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                db.CombiLogs.Add(new CombiLog()
                {
                    DeviceId = deviceId,
                    State = state,
                    Date = Now
                });
                await db.SaveChangesAsync();
            }
        }

        public static async Task<List<CombiLogDto>> GetDtos(string ownedDeviceId, int lastHours = 12, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var afterDate = Now.AddHours(-lastHours);

                var query = db.CombiLogs.Where(x => x.DeviceId == ownedDeviceId && x.Date > afterDate).OrderByDescending(x => x.Id);
                if ((await query.CountAsync()) < 10)
                    query = db.CombiLogs.Where(x => x.DeviceId == ownedDeviceId).Take(10).OrderByDescending(x => x.Id);

                return await query.Select(x => new CombiLogDto()
                {
                    Date = x.Date,
                    State = x.State
                }).ToListAsync();
            }
        }
    }
}
