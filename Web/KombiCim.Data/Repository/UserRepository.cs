using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Utilities;
using KombiCim.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Repository
{
    public class UserRepository : BaseRepository
    {
        public static async Task<User> Get(string emailAddress, string password, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;
                var hashPassword = StringHelper.Hash(password);
                return await db.Users.Where(x => x.EmailAddress == emailAddress && x.Password == hashPassword).SingleOrDefaultAsync();
            }
        }

        public static User Get(string token, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var apiToken = db.ApiTokens.Where(x => x.Token == token && x.Active).SingleOrDefault();
                return apiToken?.User;
            }
        }

        public static async Task<string> GetToken(string encodedUserInfo, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUserInfo));
                var usernamePasswordArray = decodedContent.Split(':');
                var username = usernamePasswordArray[0];
                var password = usernamePasswordArray[1];

                var user = await Get(username, password, db);
                if (user != null)
                {
                    var currentToken = await db.ApiTokens.Where(x => x.UserId == user.Id && x.Active).SingleOrDefaultAsync();
                    if (currentToken == null)
                    {
                        currentToken = new ApiToken()
                        {
                            Token = StringHelper.Random(16),
                            CreatedAt = Now,
                            Active = true,
                            UserId = user.Id
                        };

                        db.ApiTokens.Add(currentToken);
                        await db.SaveChangesAsync();
                    }

                    return currentToken.Token;
                }
                else
                    return null;
            }
        }
    }

}
