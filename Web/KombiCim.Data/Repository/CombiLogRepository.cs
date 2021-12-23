using Kombicim.Data.Utilities;
using Kombicim.Data.Models.Mobile.Dtos;
using Microsoft.EntityFrameworkCore;
using Kombicim.Data.Entities;

namespace Kombicim.Data.Repository
{
    public class CombiLogRepository : BaseRepository
    {
        public CombiLogRepository(KombicimDataContext kombiCimDataContext) : base(kombiCimDataContext)
        {
        }

        public async Task Post(string deviceId, bool state)
        {
            Db.CombiLogs.Add(new CombiLogEntity()
            {
                DeviceId = deviceId,
                State = state,
                CreatedAt = Now
            });
            await Db.SaveChangesAsync();
        }

        public async Task<List<CombiLogDto>> GetDtos(string ownedDeviceId, int lastHours = 12)
        {
            var afterDate = Now.AddHours(-lastHours);

            var query = Db.CombiLogs.Where(x => x.DeviceId == ownedDeviceId && x.CreatedAt > afterDate).OrderByDescending(x => x.Id);
            if ((await query.CountAsync()) < 10)
                query = Db.CombiLogs.Where(x => x.DeviceId == ownedDeviceId).Take(10).OrderByDescending(x => x.Id);

            return await query.Select(x => new CombiLogDto()
            {
                Date = x.CreatedAt,
                State = x.State
            }).ToListAsync();
        }
    }
}
