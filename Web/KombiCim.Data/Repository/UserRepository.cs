using Kombicim.Data.Entities;
using Kombicim.Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Kombicim.Data.Repository
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(KombicimDataContext kombiCimDataContext) : base(kombiCimDataContext)
        {

        }

        public async Task<UserEntity> Get(string emailAddress, string password)
        {
            var hashPassword = StringHelper.Hash(password);
            return await Db.Users.Where(x => x.EmailAddress == emailAddress && x.Password == hashPassword).SingleOrDefaultAsync();

        }

        public async Task<UserEntity> Get(string token)
        {
            var apiToken = await Db.ApiTokens.Where(x => x.Token == token && x.Active).SingleOrDefaultAsync();
            return apiToken?.User;
        }

        public async Task<UserEntity> GetByOwnedDeviceId(string deviceId)
        {
            return await Db.Users.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
        }

        public async Task<string> GetToken(string encodedUserInfo)
        {
            var decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUserInfo));
            var usernamePasswordArray = decodedContent.Split(':');
            var username = usernamePasswordArray[0];
            var password = usernamePasswordArray[1];

            var user = await Get(username, password);
            if (user != null)
            {
                var currentToken = await Db.ApiTokens.Where(x => x.UserId == user.Id && x.Active).SingleOrDefaultAsync();
                if (currentToken == null)
                {
                    currentToken = new ApiTokenEntity()
                    {
                        Token = StringHelper.Random(16),
                        CreatedAt = Now,
                        Active = true,
                        UserId = user.Id
                    };

                    Db.ApiTokens.Add(currentToken);
                    await Db.SaveChangesAsync();
                }

                return currentToken.Token;
            }
            else
                return null;
        }
    }
}
