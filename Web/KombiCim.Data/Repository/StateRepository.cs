using Kombicim.Data.Entities;
using Kombicim.Data.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Kombicim.Data.Repository
{
    public class StateRepository : BaseRepository
    {
        public StateRepository(KombicimDataContext kombiCimDataContext) : base(kombiCimDataContext)
        {

        }

        public async Task<bool> Get(string deviceId)
        {
            var state = await Db.States.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
            if (state != null)
                return state.Value;
            else
            {
                state = new CombiStateEntity()
                {
                    DeviceId = deviceId,
                    Value = true,
                    CreatedAt = Now
                };
                Db.States.Add(state);
                await Db.SaveChangesAsync();

                return true;
            }
        }

        public async Task Set(string deviceId, bool value)
        {

            var state = await Db.States.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
            if (state != null)
                state.Value = value;
            else
                Db.States.Add(new CombiStateEntity()
                {
                    DeviceId = deviceId,
                    Value = value,
                    CreatedAt = Now
                });

            await Db.SaveChangesAsync();
        }
    }
}
