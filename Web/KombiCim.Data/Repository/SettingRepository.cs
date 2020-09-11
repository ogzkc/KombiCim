using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Exceptions;
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
    public class SettingRepository : BaseRepository
    {
        public static async Task<SettingsDto> GetActiveDto(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;
                var settings = new SettingsDto();

                var activeProfile = await db.Profiles.Where(x => x.User.OwnedDeviceId == deviceId && x.Active).SingleOrDefaultAsync();
                if (activeProfile == null)
                {
                    var device = await DeviceRepository.Get(deviceId, db);
                    if (device.CenterDeviceId != null)
                        throw RepositoryException.NotCenterDevice(deviceId);

                    var profiles = await db.Profiles.Where(x => x.User.OwnedDeviceId == deviceId).ToListAsync();
                    if (profiles.Any())
                    {
                        // profiles exist but none of them is active | make active one of them ( ???? )
                        activeProfile = profiles.FirstOrDefault();
                        activeProfile.Active = true;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        // profiles not exist create default profiles for first time use
                        await CreateDefaultProfiles(deviceId);
                    }
                }

                settings.Guid = await GetGuid(deviceId);
                settings.ProfileName = activeProfile.Name;
                settings.Mode = activeProfile.ProfileType.Name;

                if (activeProfile.ProfileType.Name == ProfileRepository.MODE_AUTO_PROFILE)
                {
                    var locationId = await LocationRepository.GetId(deviceId, db);
                    var minTempValue = await MinTemperatureRepository.Get(locationId, activeProfile.Id, db_: db);

                    if (minTempValue == null)
                        throw new RepositoryException($"{locationId} id'li Location'ın default MinTemp bilgisi bulunamadı.");

                    settings.MinTemperature = minTempValue.Value;
                }
                else if (activeProfile.ProfileType.Name == ProfileRepository.MODE_MANUAL)
                {
                    settings.State = await StateRepository.Get(deviceId, db);
                }
                else if (activeProfile.ProfileType.Name == ProfileRepository.MODE_AUTO_PROFILE_SCHEDULED_1)
                {
                    // todo: fix it
                    settings.ProfileDtos = await ProfileRepository.GetDtos(deviceId, db_: db);
                    settings.ProfilePreferenceDtos = await ProfileRepository.GetProfilePreferenceDtos(activeProfile.Id, db_: db);
                }
                else if (activeProfile.ProfileType.Name == ProfileRepository.MODE_AUTO_SCHEDULED_1)
                {
                    // todo: fix it
                    var locationId = await LocationRepository.GetId(deviceId, db);
                    settings.MinTemperatureDtos = await MinTemperatureRepository.GetDtos(locationId, activeProfile.Id, db);
                }

                return settings;
            }
        }

        private static Task CreateDefaultProfiles(string deviceId)
        {
            throw new NotImplementedException();
        }

        public static async Task<string> GetGuid(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var random = string.Empty;

                var setting = await db.Settings.Where(x => x.DeviceId == deviceId && x.DisabledAt == null).SingleOrDefaultAsync();
                if (setting == null)
                    random = await PostRandomGuid(deviceId, db);
                else
                    random = setting.Id;

                return random;
            }
        }

        public static async Task<string> PostRandomGuid(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var oldSettings = await db.Settings.Where(x => x.DeviceId == deviceId && x.DisabledAt == null).SingleOrDefaultAsync();
                if (oldSettings != null)
                    db.Settings.Remove(oldSettings);

                var random = StringHelper.Random(StringHelper.GUID_LENGTH);
                db.Settings.Add(new Setting()
                {
                    Id = random,
                    CreatedAt = Now,
                    DeviceId = deviceId
                });
                await db.SaveChangesAsync();

                return random;
            }
        }

        public static async Task SetState(string deviceId, bool state, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var currentState = await db.States.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
                if (currentState == null)
                {
                    currentState = new State()
                    {
                        DeviceId = deviceId,
                        CreatedAt = Now,
                        Value = state
                    };
                    db.States.Add(currentState);
                }
                else
                    currentState.Value = state;

                await db.SaveChangesAsync();
            }
        }

        public static async Task PostMinTemperature(string deviceId, float value, int? dayOfWeek = null, int? hour = null, int? minute = null, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var location = await LocationRepository.Get(deviceId, db);
                db.MinTemperatures.Add(new MinTemperature()
                {
                    Value = value,
                    LocationId = location.Id,
                    CreatedAt = DateTime.Now,
                    DayOfWeek = dayOfWeek,
                    Hour = hour,
                    Minute = minute
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
