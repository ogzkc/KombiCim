using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Utilities;
using KombiCim.Data.Exceptions;
using KombiCim.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KombiCim.Data.Models;

namespace KombiCim.Data.Repository
{
    public class LocationRepository : BaseRepository
    {
        public const string DEFAULT_LOCATION = "Oda 1";


        public static async Task<Location> Get(int id, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var location = await db.Locations.Where(x => x.Id == id && x.Active).SingleOrDefaultAsync();
                return location;
            }
        }

        public static async Task<Location> Get(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var location = await db.Locations.Where(x => x.DeviceId == deviceId && x.Active).SingleOrDefaultAsync();
                if (location == null)
                    throw new RepositoryException($"{deviceId} id'li device'ın location nesnesi bulunamadı.")
                    {
                        RepositoryName = "Location",
                        ExceptionType = RepositoryExceptionType.LocationNotExist,
                        Value = deviceId
                    };

                return location;
            }
        }

        public static async Task<int> GetId(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var locationId = await db.Locations.Where(x => x.DeviceId == deviceId && x.Active).Select(x => x.Id).SingleOrDefaultAsync();
                if (locationId <= 0)
                    throw new RepositoryException($"{deviceId} id'li device'ın location nesnesi bulunamadı.")
                    {
                        RepositoryName = "Location",
                        ExceptionType = RepositoryExceptionType.LocationNotExist,
                        Value = deviceId
                    };

                return locationId;
            }
        }

        public static async Task<List<LocationDto>> GetLocationDtos(string deviceId, int profileId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var locationDtos = new List<LocationDto>();

                locationDtos.Add(await GetLocationDto(deviceId, profileId, db));

                var devices = await db.Devices.Where(x => x.CenterDeviceId == deviceId).ToListAsync();
                foreach (var device in devices)
                {
                    locationDtos.Add(await GetLocationDto(device.Id, profileId, db));
                }


                return locationDtos;
            }
        }


        public static async Task<LocationDto> GetLocationDto(string deviceId, int profileId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var location = await Get(deviceId, db);
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
                    DeviceTypeName = location.Device.DeviceType.Name
                };

                var minTemps = await db.MinTemperatures.Where(x => x.LocationId == location.Id && x.ProfileId == profileId && x.DisabledAt == null).ToListAsync();
                if (minTemps.Count == 1)
                    locationDto.MinTempValue = minTemps.SingleOrDefault().Value;
                else if (minTemps.Count > 1)
                {
                    locationDto.MinTemperatures = minTemps.Select(x => new MinTemperatureDto()
                    {
                        Hour = x.Hour,
                        Minute = x.Minute,
                        DayOfWeek = x.DayOfWeek,
                        Value = x.Value
                    }).ToList();
                }
                /*
                else
                    throw new RepositoryException($"{location.Id} id'li Location'ın {profileId} id'li profilinde aktif MinTemp bulunamadı.")
                    {
                        RepositoryName = nameof(db.MinTemperatures),
                        ExceptionType = RepositoryExceptionType.MinTempNotExist
                    };
                */

                return locationDto;
            }
        }

        public static async Task<Location> Post(string deviceId, string name, bool createMinTemp = false, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var location = new Location()
                {
                    Name = name,
                    DeviceId = deviceId,
                    Active = true,
                    CreatedAt = Now
                };
                db.Locations.Add(location);

                await db.SaveChangesAsync();
                if (createMinTemp)
                    await MinTemperatureRepository.Post(location.Id, MinTemperatureRepository.DEFAULT_MIN_TEMP, db_: db);

                return location;
            }
        }
    }
}
