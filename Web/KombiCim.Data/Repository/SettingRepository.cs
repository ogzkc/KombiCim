using Kombicim.Data.Exceptions;
using Kombicim.Data.Utilities;
using Microsoft.EntityFrameworkCore;
using Kombicim.Data.Models;
using Kombicim.Data.Entities;
using Kombicim.Data.Models.Arduino.Dtos;

namespace Kombicim.Data.Repository
{
    public class SettingRepository : BaseRepository
    {
        private readonly KombicimDataContext kombiCimDataContext;
        private readonly StateRepository stateRepository;
        private readonly ProfileRepository profileRepository;
        private readonly WeatherRepository weatherRepository;

        public SettingRepository(KombicimDataContext kombiCimDataContext, StateRepository stateRepository, ProfileRepository profileRepository, WeatherRepository weatherRepository) : base(kombiCimDataContext)
        {
            this.kombiCimDataContext = kombiCimDataContext;
            this.stateRepository = stateRepository;
            this.profileRepository = profileRepository;
            this.weatherRepository = weatherRepository;
        }


        public async Task<SettingsDto> GetActiveDto(string deviceId)
        {
            var settings = new SettingsDto();

            var activeProfile = await Db.Profiles.Where(x => x.User.DeviceId == deviceId && x.Active).SingleOrDefaultAsync();
            if (activeProfile == null)
            {
                var device = await Db.Devices.Where(x => x.Id == deviceId).SingleOrDefaultAsync();
                if (device.CenterDeviceId != null)
                    throw RepositoryException.NotCenterDevice(deviceId);

                var profiles = await Db.Profiles.Where(x => x.User.DeviceId == deviceId).ToListAsync();
                if (profiles.Any())
                {
                    // profiles exist but none of them is active | make active one of them ( ???? )
                    activeProfile = profiles.FirstOrDefault();
                    activeProfile.Active = true;
                    await Db.SaveChangesAsync();
                }
                else
                {
                    // profiles not exist create default profiles for first time use
                    await CreateDefaultProfiles(deviceId);
                }
            }

            settings.Guid = await GetGuid(deviceId);
            settings.ProfileName = activeProfile.Name;
            settings.Mode = ProfileType.GetName(activeProfile.TypeId);

            if (activeProfile.TypeId == ProfileType.MODE_AUTO_PROFILE_ID)
            {
                var locationId = await Db.Locations.Where(x => x.DeviceId == deviceId && x.Active).Select(x => x.Id).SingleOrDefaultAsync();
                var minTempValue = await Db.MinTemperatures.Where(x => x.LocationId == locationId && x.ProfileId == activeProfile.Id).SingleOrDefaultAsync();

                if (minTempValue == null)
                    throw new RepositoryException($"{locationId} id'li Location'ın default MinTemp bilgisi bulunamadı.");

                settings.MinTemperature = minTempValue.Value;
            }
            else if (activeProfile.TypeId == ProfileType.MODE_MANUAL_ID)
            {
                settings.State = await stateRepository.Get(deviceId);
            }
            else if (activeProfile.TypeId == ProfileType.MODE_AUTO_SERVER_PROFILE_ID)
            {
                var minTemp = await Db.MinTemperatures.Where(x => x.ProfileId == activeProfile.Id).SingleOrDefaultAsync(); // location bilinmediği için locationId verilmeden sorgulanıyor
                if (minTemp == null)
                    throw new RepositoryException($"{activeProfile.Id} id'li Profile'ın MinTemp bilgisi bulunamadı.");

                var latestWeather = await weatherRepository.GetLastMinutes(minTemp.LocationId, 30);
                if (latestWeather == null || latestWeather.Count == 0)
                    settings.State = true;
                else
                {
                    var weathers = latestWeather.Take(2).ToList();
                    var averageWeather = weathers.Sum(x => x.Temperature) / (double)weathers.Count();
                    decimal difference = (decimal)averageWeather - (decimal)minTemp.Value;
                    if (difference <= -0.1m)
                        settings.State = true;
                    else if (difference >= 0.12m)
                        settings.State = false;
                    else
                        settings.State = true;
                }
            }
            else if (activeProfile.TypeId == ProfileType.MODE_AUTO_PROFILE_SCHEDULED_1_ID)
            {
                // todo: fix it
                settings.ProfileDtos = await profileRepository.GetDtos(deviceId);
                settings.ProfilePreferenceDtos = await profileRepository.GetProfilePreferenceDtos(activeProfile.Id);
            }
            else if (activeProfile.TypeId == ProfileType.MODE_AUTO_SCHEDULED_1_ID)
            {
                // todo: fix it
                var locationId = await Db.Locations.Where(x => x.DeviceId == deviceId && x.Active).Select(x => x.Id).SingleOrDefaultAsync();
                settings.MinTemperatureDtos = await Db.MinTemperatures.Where(x => x.LocationId == locationId && x.ProfileId == activeProfile.Id).Select(x => new MinTemperatureDto() { Value = x.Value }).ToListAsync();
            }

            return settings;
        }

        private static Task CreateDefaultProfiles(string deviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetGuid(string deviceId)
        {
            var random = string.Empty;

            var setting = await Db.Settings.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
            if (setting == null)
                random = await PostRandomGuid(deviceId);
            else
                random = setting.Id;

            return random;
        }

        public async Task SetState(string deviceId, bool state)
        {
            var currentState = await Db.States.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
            if (currentState == null)
            {
                currentState = new CombiStateEntity()
                {
                    DeviceId = deviceId,
                    CreatedAt = Now,
                    Value = state
                };
                Db.States.Add(currentState);
            }
            else
                currentState.Value = state;

            await Db.SaveChangesAsync();
        }

        public async Task PostMinTemperature(string deviceId, float value)
        {
            var location = await Db.Locations.Where(x => x.DeviceId == deviceId && x.Active).SingleOrDefaultAsync();
            Db.MinTemperatures.Add(new MinTemperatureEntity()
            {
                Value = value,
                LocationId = location.Id,
                CreatedAt = DateTime.Now
            });
            await Db.SaveChangesAsync();
        }
    }
}
