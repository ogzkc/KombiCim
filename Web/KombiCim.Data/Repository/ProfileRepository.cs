using Microsoft.EntityFrameworkCore;
using Kombicim.Data.Exceptions;
using Kombicim.Data.Models.Mobile.Dtos;
using Kombicim.Data.Entities;
using Kombicim.Data.Models;
using Kombicim.Data.Models.Arduino.Dtos;

namespace Kombicim.Data.Repository
{
    public class ProfileRepository : BaseRepository
    {
        private const string DEFAULT_PROFILE_1 = "Gündüz";
        private const string DEFAULT_PROFILE_2 = "Gece";
        private const string DEFAULT_PROFILE_3 = "Dışarı";

        private readonly MinTemperatureRepository minTemperatureRepository;
        private readonly StateRepository stateRepository;
        private const double DEFAULT_TEMP = 24.0;
        public const string DEFAULT_MODE = ProfileType.MODE_AUTO_PROFILE;

        public ProfileRepository(KombicimDataContext kombiCimDataContext, MinTemperatureRepository minTemperatureRepository, StateRepository stateRepository) : base(kombiCimDataContext)
        {
            this.minTemperatureRepository = minTemperatureRepository;
            this.stateRepository = stateRepository;
        }

        public async Task<ProfileEntity> Get(int id) => await Db.Profiles.Where(x => x.Id == id).SingleOrDefaultAsync();

        public List<ProfileTypeDto> GetSupportedProfileTypeDtos()
        {
            return ProfileType.All.Select(x => new ProfileTypeDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ServerBased = x.ServerBased
            }).ToList();
        }

        public async Task<ProfileEntity> Create(string name, int profileTypeId, int userId)
        {
            var exist = await Db.Profiles.Where(x => x.Name == name && x.Active).AnyAsync();
            if (exist)
                throw new RepositoryException($"Zaten {name} isimli bir profiliniz bulunmaktadır.");

            return await Db.Profiles.Where(x => x.Id == profileTypeId).SingleOrDefaultAsync();
        }

        public async Task<ProfileEntity> GetActive(int userId) => await Db.Profiles.Where(x => x.UserId == userId && x.Active).SingleOrDefaultAsync();

        public async Task SetActive(int profileId, int userId)
        {
            var profile = await Db.Profiles.Where(x => x.Id == profileId && x.UserId == userId).SingleOrDefaultAsync();
            if (profile == null)
                throw new RepositoryException($"{userId} id'li user'a ait {profileId} id'li profil bulunamadı.");

            profile.Active = true;

            var profiles = await Db.Profiles.Where(x => x.UserId == userId && x.Id != profileId).ToListAsync();
            foreach (var disablingProfile in profiles)
                disablingProfile.Active = false;

            await Db.SaveChangesAsync();

            await PostRandomGuid(profile.User.DeviceId);
        }

        public async Task<List<ProfileDto>> GetDtos(string deviceId, int locationId = -1)
        {
            if (locationId == -1)
                locationId = await Db.Locations.Where(x => x.DeviceId == deviceId && x.Active).Select(x => x.Id).SingleOrDefaultAsync();

            var profiles = await Db.Profiles.Where(x => x.User.DeviceId == deviceId).ToListAsync();

            var profileDtos = new List<ProfileDto>();

            foreach (var profile in profiles)
            {
                var profileDto = new ProfileDto();

                bool? state = null;
                MinTemperatureEntity minTemp = null;
                if (profile.TypeId == ProfileType.MODE_AUTO_PROFILE_ID)
                {
                    minTemp = await minTemperatureRepository.Get(locationId, profile.Id);
                    if (minTemp == null)
                    {
                        minTemp = await minTemperatureRepository.Post(locationId, DEFAULT_TEMP, profile.Id);
                        if (minTemp == null)
                            throw new RepositoryException($"{profile.Id} 'idli profil ve {locationId} id'li location için MinTemp bilgisi yok ve yenisi oluşturulamıyor.");
                    }
                }
                else if (profile.TypeId == ProfileType.MODE_AUTO_SERVER_PROFILE_ID)
                {
                    minTemp = await minTemperatureRepository.Get(profile.Id);
                    if (minTemp == null)
                        throw new RepositoryException($"{profile.Id} 'idli profil ve {locationId} id'li location için MinTemp bilgisi yok ve yenisi oluşturulamıyor.");

                    var location = await Db.Locations.Where(x => x.Id == minTemp.LocationId && x.Active).SingleOrDefaultAsync();
                    profileDto.SelectedThermometer = new LocationDto()
                    {
                        Id = location.Id,
                        Name = location.Name,
                        DeviceId = location.DeviceId,
                        DeviceTypeName = DeviceType.GetName(location.Device.TypeId),
                        MinTempValue = minTemp.Value
                    };
                }
                if (profile.TypeId == ProfileType.MODE_MANUAL_ID)
                {
                    state = await stateRepository.Get(deviceId);
                }


                profileDto.Id = profile.Id;
                profileDto.MinTempValue = minTemp?.Value;
                profileDto.ProfileName = profile.Name;
                profileDto.TypeName = ProfileType.GetName(profile.TypeId);
                profileDto.State = state;
                profileDto.Active = profile.Active;

                profileDtos.Add(profileDto);
            }

            return profileDtos;
        }

        public async Task<List<ProfilePreferenceDto>> GetProfilePreferenceDtos(int profileId)
        {
            return await Db.ProfilePreferences.Where(x => x.ProfileId == profileId && x.Active).Select(x => new ProfilePreferenceDto()
            {
                // TODO
            }).ToListAsync();
        }
    }
}
