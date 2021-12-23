using Kombicim.Data.Utilities;
using Kombicim.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Kombicim.Data.Entities;
using Kombicim.Data.Models;

namespace Kombicim.Data.Repository
{
    public class DeviceRepository : BaseRepository
    {
        private readonly LocationRepository locationRepository;

        public DeviceRepository(LocationRepository locationRepository, KombicimDataContext kombiCimDataContext) : base(kombiCimDataContext)
        {
            this.locationRepository = locationRepository;
        }

        public async Task<DeviceEntity> Get(string deviceId)
        {
            return await Db.Devices.Where(x => x.Id == deviceId).SingleOrDefaultAsync();
        }

        public async Task<DeviceEntity> Post(string deviceId, string typeName, string centerDeviceId = null, string initialLocation = null)
        {
            if (!await Exist(deviceId))
            {
                if (typeName != DeviceType.CENTER_NAME || typeName != DeviceType.THERMOMETER_NAME)
                    throw new RepositoryException($"Invalid device type name: {typeName} | Supported device type names: {string.Join(",", DeviceType.All.Select(x => x.Name))}");

                typeName = typeName.ToLowerInvariant();

                var type = DeviceType.All.Where(x => x.Name == typeName).SingleOrDefault();
                if (type == null)
                    throw new RepositoryException($"'{typeName}' typeName parametresi ile bir DeviceType bulunamadı.");

                var device = new DeviceEntity
                {
                    Id = deviceId,
                    TypeId = type.Id,
                    CenterDeviceId = centerDeviceId,
                    CreatedAt = Now
                };
                Db.Devices.Add(device);
                await Db.SaveChangesAsync();

                if (initialLocation.NullEmpty())
                    initialLocation = LocationRepository.DEFAULT_LOCATION;

                await locationRepository.Post(deviceId, initialLocation, centerDeviceId == null);

                return device;
            }
            else
                return await Get(deviceId);
        }


        public async Task<bool> Exist(string deviceId)
        {
            return await Db.Devices.Where(x => x.Id == deviceId).AnyAsync();
        }
    }

}
