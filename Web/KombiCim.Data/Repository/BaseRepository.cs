using Kombicim.Data.Entities;
using Kombicim.Data.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Kombicim.Data.Repository
{
    public class BaseRepository
    {
        protected KombicimDataContext Db { get; }

        public BaseRepository(KombicimDataContext kombiCimDataContext)
        {
            Db = kombiCimDataContext;
        }

        protected static DateTime Now
        {
            get
            {
                var now = DateTime.Now;
                return now.AddMilliseconds(-now.Millisecond).ToUniversalTime();
            }
        }

        public async Task<string> PostRandomGuid(string deviceId)
        {
            var oldSettings = await Db.Settings.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
            if (oldSettings != null)
                Db.Settings.Remove(oldSettings);

            var random = StringHelper.Random(StringHelper.GUID_LENGTH);
            Db.Settings.Add(new SettingEntity()
            {
                Id = random,
                CreatedAt = Now,
                DeviceId = deviceId
            });
            await Db.SaveChangesAsync();

            return random;
        }
    }
}
