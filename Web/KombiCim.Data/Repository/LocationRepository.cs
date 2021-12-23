using Kombicim.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Kombicim.Data.Entities;
using Kombicim.Data.Models.Arduino.Dtos;
using Kombicim.Data.Models;

namespace Kombicim.Data.Repository
{
    public class LocationRepository : BaseRepository
    {
        public const string DEFAULT_LOCATION = "Oda 1"; // "Room 1"
        private readonly MinTemperatureRepository minTemperatureRepository;

        public LocationRepository(KombicimDataContext kombiCimDataContext, MinTemperatureRepository minTemperatureRepository) : base(kombiCimDataContext)
        {
            this.minTemperatureRepository = minTemperatureRepository;
        }

        public async Task<LocationEntity> Get(int id)
        {
            var location = await Db.Locations.Where(x => x.Id == id && x.Active).SingleOrDefaultAsync();
            return location;
        }

        public async Task<LocationEntity> Get(string deviceId)
        {
            var location = await Db.Locations.Where(x => x.DeviceId == deviceId && x.Active).SingleOrDefaultAsync();
            if (location == null)
                throw new RepositoryException($"{deviceId} id'li device'ın location nesnesi bulunamadı.")
                {
                    RepositoryName = "Location",
                    ExceptionType = RepositoryExceptionType.LocationNotExist,
                    Value = deviceId
                };

            return location;
        }

        public async Task<int> GetId(string deviceId)
        {
            var locationId = await Db.Locations.Where(x => x.DeviceId == deviceId && x.Active).Select(x => x.Id).SingleOrDefaultAsync();
            if (locationId <= 0)
                throw new RepositoryException($"{deviceId} id'li device'ın location nesnesi bulunamadı.")
                {
                    RepositoryName = "Location",
                    ExceptionType = RepositoryExceptionType.LocationNotExist,
                    Value = deviceId
                };

            return locationId;
        }


        public async Task<List<LocationDto>> GetLocationDtos(string deviceId, int profileId)
        {
            var locationDtos = new List<LocationDto>();

            locationDtos.Add(await GetLocationDto(deviceId, profileId));

            var devices = await Db.Devices.Where(x => x.CenterDeviceId == deviceId).ToListAsync();
            foreach (var device in devices)
            {
                locationDtos.Add(await GetLocationDto(device.Id, profileId));
            }

            return locationDtos;
        }

        public async Task<List<LocationDto>> GetThermometers(string centerDeviceId)
        {
            var locationDtos = new List<LocationDto>();

            var devices = await Db.Devices.Where(x => x.TypeId == DeviceType.THERMOMETER_ID && x.CenterDeviceId == centerDeviceId).ToListAsync();
            foreach (var device in devices)
            {
                locationDtos.Add(await GetLocationDto(device.Id));
            }

            return locationDtos;
        }


        public async Task<LocationDto> GetLocationDto(string deviceId, int profileId)
        {
            var location = await Get(deviceId);
            if (location == null)
                throw new RepositoryException($"{deviceId} id'li device'ın location nesnesi bulunamadı.")
                {
                    RepositoryName = "Location",
                    ExceptionType = RepositoryExceptionType.LocationNotExist,
                    Value = deviceId
                };

            var locationDto = new LocationDto()
            {
                Id = location.Id,
                DeviceId = deviceId,
                Name = location.Name,
                DeviceTypeName = DeviceType.GetName(location.Device.TypeId)
            };

            var minTemps = await Db.MinTemperatures.Where(x => x.LocationId == location.Id && x.ProfileId == profileId).ToListAsync();
            if (minTemps.Count == 1)
                locationDto.MinTempValue = minTemps.SingleOrDefault().Value;
            else if (minTemps.Count > 1)
            {
                // TODO: for future (scheduling feature)
                //locationDto.MinTemperatures = minTemps.Select(x => new MinTemperatureDto()
                //{
                //    Hour = x.Hour,
                //    Minute = x.Minute,
                //    DayOfWeek = x.DayOfWeek,
                //    Value = x.Value
                //}).ToList();
            }
            /*
            else
                throw new RepositoryException($"{location.Id} id'li Location'ın {profileId} id'li profilinde aktif MinTemp bulunamadı.")
                {
                    RepositoryName = nameof(Db.MinTemperatures),
                    ExceptionType = RepositoryExceptionType.MinTempNotExist
                };
            */

            return locationDto;
        }


        public async Task<LocationDto> GetLocationDto(string deviceId)
        {
            var location = await Get(deviceId);
            if (location == null)
                throw new RepositoryException($"{deviceId} id'li device'ın location nesnesi bulunamadı.")
                {
                    RepositoryName = "Location",
                    ExceptionType = RepositoryExceptionType.LocationNotExist,
                    Value = deviceId
                };

            var locationDto = new LocationDto()
            {
                Id = location.Id,
                DeviceId = deviceId,
                Name = location.Name,
                DeviceTypeName = DeviceType.GetName(location.Device.TypeId)
            };

            return locationDto;
        }

        public async Task<LocationEntity> Post(string deviceId, string name, bool createMinTemp = false)
        {
            var location = new LocationEntity()
            {
                Name = name,
                DeviceId = deviceId,
                Active = true,
                CreatedAt = Now
            };
            Db.Locations.Add(location);

            var user = await Db.Users.Where(x => x.DeviceId == deviceId).SingleOrDefaultAsync();
            var activeProfile = await Db.Profiles.Where(x => x.UserId == user.Id && x.Active).SingleOrDefaultAsync();

            await Db.SaveChangesAsync();
            if (createMinTemp)
                await minTemperatureRepository.Post(location.Id, MinTemperatureRepository.DEFAULT_MIN_TEMP, activeProfile.Id);

            return location;
        }
    }
}

