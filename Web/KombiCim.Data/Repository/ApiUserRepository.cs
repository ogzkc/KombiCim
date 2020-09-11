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
    public class ApiUserRepository : BaseRepository
    {
        public static ApiUser Get(string username, string password, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                return db.ApiUsers.Where(x => x.Username == username && x.Password == password && x.Active).SingleOrDefault();
            }
        }
    }
}
