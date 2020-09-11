using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KombiCim.Data.Repository
{
    public class StateRepository : BaseRepository
    {
        public static async Task<bool> Get(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var state = await db.States.Where(x => x.DeviceId == deviceId && x.DisabledAt == null).SingleOrDefaultAsync();
                if (state != null)
                    return state.Value;
                else
                {
                    state = new State()
                    {
                        DeviceId = deviceId,
                        Value = true,
                        CreatedAt = Now
                    };
                    db.States.Add(state);
                    await db.SaveChangesAsync();

                    return true;
                }
            }
        }

        public static async Task Set(string deviceId, bool value, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var state = await db.States.Where(x => x.DeviceId == deviceId && x.DisabledAt == null).SingleOrDefaultAsync();
                if (state != null)
                    state.DisabledAt = Now;

                var newState = new State()
                {
                    DeviceId = deviceId,
                    Value = value,
                    CreatedAt = Now
                };
                db.States.Add(newState);

                await db.SaveChangesAsync();
            }
        }
    }
}
