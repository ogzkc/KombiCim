using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Repository
{
    public class MinOperatorRepository : BaseRepository
    {
        public static async Task<string> GetActive(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var minOperator = await db.MinOperators.Where(x => x.DeviceId == deviceId && x.DeletedAt == null).SingleOrDefaultAsync();

                return minOperator != null ? minOperator.Value : string.Empty;
            }
        }
    }
}
