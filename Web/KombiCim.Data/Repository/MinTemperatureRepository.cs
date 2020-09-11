using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Utilities;
using KombiCim.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KombiCim.Data.Models;

namespace KombiCim.Data.Repository
{
    public class MinTemperatureRepository : BaseRepository
    {
        public static double DEFAULT_MIN_TEMP = 24.0;
        private const string NAME = "MinTemperature";

        public static async Task<MinTemperature> Get(int locationId, int profileId, int? dayOfWeek = null, int? hour = null, int? minute = null, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;
                return await db.MinTemperatures.Where(x => x.LocationId == locationId && x.ProfileId == profileId && x.Hour == hour && x.Minute == minute && x.DayOfWeek == dayOfWeek && x.DisabledAt == null).SingleOrDefaultAsync();
            }
        }

        public static async Task<MinTemperatureDto> GetDto(int locationId, int profileId, int? dayOfWeek = null, int? hour = null, int? minutes = null, KombiCimEntities db_ = null)
        {
            var minTemperature = await Get(locationId, profileId, dayOfWeek, hour, minutes, db_);
            if (minTemperature != null)
                return new MinTemperatureDto()
                {
                    Value = minTemperature.Value,
                    DayOfWeek = minTemperature.DayOfWeek,
                    Hour = minTemperature.Hour,
                    Minute = minTemperature.Minute
                };
            else
                return null;
        }

        public static async Task<List<MinTemperatureDto>> GetDtos(int locationId, int profileId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;
                var minTemperatures = await db.MinTemperatures.Where(x => x.LocationId == locationId && x.ProfileId == profileId && x.DisabledAt == null).ToListAsync();

                return minTemperatures.Select(x => new MinTemperatureDto()
                {
                    Value = x.Value,
                    DayOfWeek = x.DayOfWeek,
                    Hour = x.Hour,
                    Minute = x.Minute
                }).ToList();
            }
        }

        public static async Task<bool> SetProfile(int profileId, double value, int? dayOfWeek = null, int? hour = null, int? minutes = null, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var profile = await ProfileRepository.Get(profileId, db);

                var deviceId = profile.User.OwnedDeviceId;
                var locations = await db.Locations.Where(x => x.DeviceId == deviceId && x.Active).Select(x => x.Id).ToListAsync();

                foreach (var locationId in locations)
                {
                    var minTemp = await Get(locationId, profileId, dayOfWeek, hour, minutes, db);
                    if (minTemp == null)
                        await Post(locationId, value, dayOfWeek, hour, minutes, db);
                    else
                    {
                        minTemp.Value = value;
                        await db.SaveChangesAsync();
                    }
                }

                await SettingRepository.PostRandomGuid(profile.User.OwnedDeviceId, db);

                return true;
            }
        }


        public static async Task<bool> Set(int profileId, int locationId, double value, int? dayOfWeek = null, int? hour = null, int? minutes = null, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var profile = await ProfileRepository.Get(profileId, db);

                var location = await LocationRepository.Get(locationId, db);
                if (location == null)
                    return false;
                else if (location.Device.OwnerUserId != profile.UserId)
                    return false;

                var minTemp = await Get(locationId, profileId, dayOfWeek, hour, minutes, db);
                if (minTemp == null)
                    await Post(locationId, value, dayOfWeek, hour, minutes, db);
                else
                {
                    minTemp.Value = value;
                    await db.SaveChangesAsync();
                }

                await SettingRepository.PostRandomGuid(location.DeviceId, db);

                return true;
            }
        }

        public static async Task<MinTemperature> Post(int locationId, double value, int? dayOfWeek = null, int? hour = null, int? minute = null, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var location = await LocationRepository.Get(locationId, db);
                if (location == null)
                    return null;
                // TODO: Solve this
                //else if (location.Device.OwnerUserId != userId)
                //    return null;

                var minTemperature = new MinTemperature()
                {
                    LocationId = locationId,
                    Value = value,
                    DayOfWeek = dayOfWeek,
                    Hour = hour,
                    Minute = minute,
                    CreatedAt = Now
                };
                db.MinTemperatures.Add(minTemperature);
                await db.SaveChangesAsync();

                await SettingRepository.PostRandomGuid(location.DeviceId, db);

                return minTemperature;
            }
        }
    }
}
