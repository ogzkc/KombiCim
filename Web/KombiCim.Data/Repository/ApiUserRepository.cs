using Kombicim.Data.Utilities;
using Kombicim.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kombicim.Data.Repository
{
    public class ApiUserRepository : BaseRepository
    {

        public ApiUserRepository(KombicimDataContext kombiCimDataContext) : base(kombiCimDataContext)
        {
        }

        public async Task<ApiUserEntity> GetAsync(string username, string password)
        {
            return await Db.ApiUsers.Where(x => x.Username == username && x.Password == password && x.Active).SingleOrDefaultAsync();
        }
    }
}
