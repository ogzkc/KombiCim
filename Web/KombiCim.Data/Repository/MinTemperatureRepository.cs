using Kombicim.Data.Utilities;
using Kombicim.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Kombicim.Data.Entities;
using Kombicim.Data.Models;
using Kombicim.Data.Models.Arduino.Dtos;

namespace Kombicim.Data.Repository
{
    public class MinTemperatureRepository : BaseRepository
    {
        public const double DEFAULT_MIN_TEMP = 24.0;

        public MinTemperatureRepository(KombicimDataContext kombiCimDataContext) : base(kombiCimDataContext)
        {
        }

        public async Task<MinTemperatureEntity> Get(int locationId, int profileId) => await Db.MinTemperatures.Where(x => x.LocationId == locationId && x.ProfileId == profileId).SingleOrDefaultAsync();

        public async Task<MinTemperatureEntity> Get(int profileId) => await Db.MinTemperatures.Where(x => x.ProfileId == profileId).SingleOrDefaultAsync();

        public async Task<MinTemperatureDto> GetDto(int profileId) => await Db.MinTemperatures.Where(x => x.ProfileId == profileId).Select(x => new MinTemperatureDto() { Value = x.Value }).SingleOrDefaultAsync();

        public async Task<List<MinTemperatureDto>> GetDtos(int locationId, int profileId) => await Db.MinTemperatures.Where(x => x.LocationId == locationId && x.ProfileId == profileId).Select(x => new MinTemperatureDto() { Value = x.Value }).ToListAsync();

        public async Task<bool> SetProfile(int profileId, double value)
        {
            var profile = await Db.Profiles.Where(x => x.Id == profileId).SingleOrDefaultAsync();
            var deviceId = profile.User.DeviceId;

            if (profile.TypeId == ProfileType.MODE_AUTO_PROFILE_ID)
            {
                var locations = await Db.Locations.Where(x => x.DeviceId == deviceId && x.Active).Select(x => x.Id).ToListAsync();
                foreach (var locationId in locations)
                {
                    var minTemp = await Get(locationId, profileId);
                    if (minTemp == null)
                        await Post(locationId, value, profileId);
                    else
                    {
                        minTemp.Value = value;
                        await Db.SaveChangesAsync();
                    }
                }
            }
            else if (profile.TypeId == ProfileType.MODE_AUTO_SERVER_PROFILE_ID)
            {
                var minTemp = await Get(profileId);
                minTemp.Value = value;
                await Db.SaveChangesAsync();
            }

            await PostRandomGuid(profile.User.DeviceId);

            return true;
        }


        public async Task<bool> SetLocationByProfileId(int profileId, int locationId)
        {
            var profile = await Db.Profiles.Where(x => x.Id == profileId && x.Active).SingleOrDefaultAsync();
            var minTemp = await Get(profileId);
            if (minTemp == null)
                throw new RepositoryException($"{profileId} id'li profilin MinTemp bilgisi bulunamadı.");

            minTemp.LocationId = locationId;
            await Db.SaveChangesAsync();

            await PostRandomGuid(profile.User.DeviceId);

            return true;
        }


        public async Task<bool> Set(int profileId, int locationId, double value)
        {
            var profile = await Db.Profiles.Where(x => x.Id == profileId && x.Active).SingleOrDefaultAsync();
            var location = await Db.Locations.Where(x => x.Id == locationId && x.Active).SingleOrDefaultAsync();
            if (location == null)
                return false;
            else if (location.Device.UserId != profile.UserId)
                return false;

            var minTemp = await Get(locationId, profileId);
            if (minTemp == null)
                await Post(locationId, value, profileId);
            else
            {
                minTemp.Value = value;
                await Db.SaveChangesAsync();
            }

            await PostRandomGuid(location.DeviceId);

            return true;
        }

        public async Task<MinTemperatureEntity> Post(int locationId, double value, int profileId)
        {
            var location = await Db.Locations.Where(x => x.Id == locationId && x.Active).SingleOrDefaultAsync();
            if (location == null)
                return null;
            // TODO: Solve this
            //else if (location.Device.OwnerUserId != userId)
            //    return null;

            var minTemperature = new MinTemperatureEntity()
            {
                LocationId = locationId,
                Value = value,
                ProfileId = profileId,
                CreatedAt = Now
            };
            Db.MinTemperatures.Add(minTemperature);
            await Db.SaveChangesAsync();

            await PostRandomGuid(location.DeviceId);

            return minTemperature;
        }
    }
}
