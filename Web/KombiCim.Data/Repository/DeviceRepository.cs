using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Utilities;
using KombiCim.Data.Exceptions;
using KombiCim.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KombiCim.Data.Models;
using System.Data.Entity;

namespace KombiCim.Data.Repository
{
    public class DeviceRepository : BaseRepository
    {
        private const string TYPE_THERMOMETER = "thermometer";
        private const string TYPE_CENTER = "center";

        public static Task<Device> Get(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;
                return db.Devices.Where(x => x.Id == deviceId).SingleOrDefaultAsync();
            }
        }

        public static async Task<Device> Post(string deviceId, string typeName, string centerDeviceId = null, string initialLocation = null, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                if (!await Exist(deviceId, db))
                {
                    if (typeName != TYPE_CENTER || typeName != TYPE_THERMOMETER)
                        throw new RepositoryException($"Invalid device type name: {typeName}");

                    typeName = typeName.ToLowerInvariant();

                    var type = await db.DeviceTypes.Where(x => x.Name == typeName).SingleOrDefaultAsync();
                    if (type == null)
                        throw new RepositoryException($"'{typeName}' typeName parametresi ile bir DeviceType bulunamadı.");

                    var device = new Device
                    {
                        Id = deviceId,
                        TypeId = type.Id,
                        CenterDeviceId = centerDeviceId,
                        CreatedAt = Now
                    };
                    db.Devices.Add(device);
                    await db.SaveChangesAsync();

                    if (initialLocation.NullEmpty())
                        initialLocation = LocationRepository.DEFAULT_LOCATION;

                    await LocationRepository.Post(deviceId, initialLocation, centerDeviceId == null, db);

                    return device;
                }
                else
                    return await Get(deviceId, db);
            }
        }

        public static async Task<bool> Exist(string deviceId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;
                return await db.Devices.Where(x => x.Id == deviceId).AnyAsync();
            }
        }
    }
}
