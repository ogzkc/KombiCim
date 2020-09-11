using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using KombiCim.Data.Exceptions;

namespace KombiCim.Data.Repository
{
    public class ProfileRepository : BaseRepository
    {
        public const string MODE_AUTO_PROFILE = "auto_profile_1"; // Sabit sıcaklığa göre profil bazlı otomatik kombi kontrolü
        public const string MODE_AUTO_SERVER_PROFILE = "auto_server_profile_1"; // Sabit sıcaklıklara fakat çoklu konumlara göre profil bazlı ve sunucu merkezli otomatik kombi kontrolü

        public const string MODE_AUTO_PROFILE_SCHEDULED_1 = "auto_profile_scheduled_1"; // Haftanın 7 günü için farklı saatlerde farklı profiller ayarlanabilen otomatik kombi kontrolü
        public const string MODE_AUTO_PROFILE_SERVER_SCHEDULED_1 = "auto_server_profile_scheduled_1"; // Haftanın 7 günü için farklı konumlar için farklı saatlerde farklı profiller ayarlanabilen, sunucu merkezli otomatik kombi kontrolü

        public const string MODE_AUTO_SCHEDULED_1 = "auto_scheduled_1"; // Haftanın 7 günü için farklı saatlerde farklı sıcaklıklarla ayarlanabilen otomatik kombi kontrolü
        public const string MODE_AUTO_SERVER_SCHEDULED_1 = "auto_server_scheduled_1"; // Haftanın 7 günü farklı konumlar için farklı saatlerde farklı sıcaklıklar ayarlanabilen, sunucu merkezli otomatik kombi kontrolü

        public const string MODE_MANUAL = "manual_1"; // Kombiyi doğrudan açık yada kapalı konumda tutma

        private const string DEFAULT_PROFILE_1 = "Gündüz";
        private const string DEFAULT_PROFILE_2 = "Gece";
        private const string DEFAULT_PROFILE_3 = "Dışarı";
        private const string DEFAULT_PROFILE_4 = "Özel";

        private readonly static string[] DEFAULT_PROFILES = { DEFAULT_PROFILE_1, DEFAULT_PROFILE_2, DEFAULT_PROFILE_3 };

        public const string DEFAULT_MODE = MODE_AUTO_PROFILE;

        public static async Task<Profile> Get(int id, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                return await db.Profiles.Where(x => x.Id == id).SingleOrDefaultAsync();
            }
        }

        public static async Task<Profile> GetActive(int userId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                return await db.Profiles.Where(x => x.UserId == userId && x.Active).SingleOrDefaultAsync();
            }
        }

        public static async Task SetActive(int profileId, int userId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var profile = await db.Profiles.Where(x => x.Id == profileId && x.UserId == userId).SingleOrDefaultAsync();
                if (profile == null)
                    throw new RepositoryException($"{userId} id'li user'a ait {profileId} id'li profil bulunamadı.");

                profile.Active = true;

                var profiles = await db.Profiles.Where(x => x.UserId == userId && x.Id != profileId).ToListAsync();
                foreach (var disablingProfile in profiles)
                    disablingProfile.Active = false;

                await db.SaveChangesAsync();

                await SettingRepository.PostRandomGuid(profile.User.OwnedDeviceId, db);
            }
        }

        public static async Task<List<ProfileDto>> GetDtos(string deviceId, int locationId = -1, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                if (locationId == -1)
                    locationId = (await LocationRepository.Get(deviceId, db)).Id;

                var profiles = await db.Profiles.Where(x => x.User.OwnedDeviceId == deviceId).ToListAsync();

                var profileDtos = new List<ProfileDto>();

                foreach (var profile in profiles)
                {
                    bool? state = null;
                    MinTemperature minTemp = null;
                    if (profile.ProfileType.Name != MODE_AUTO_SCHEDULED_1 &&
                        profile.ProfileType.Name != MODE_AUTO_SERVER_SCHEDULED_1 &&
                        profile.ProfileType.Name != MODE_MANUAL)
                    {
                        minTemp = await MinTemperatureRepository.Get(locationId, profile.Id, db_: db);
                        if (minTemp == null)
                            throw new RepositoryException($"{profile.Id} 'idli profil ve {locationId} id'li location için MinTemp bilgisi yok.");
                    }

                    if (profile.ProfileType.Name == MODE_MANUAL)
                    {
                        state = await StateRepository.Get(deviceId, db);
                    }

                    profileDtos.Add(new ProfileDto()
                    {
                        Id = profile.Id,
                        MinTempValue = minTemp?.Value,
                        ProfileName = profile.Name,
                        TypeName = profile.ProfileType.Name,
                        State = state,
                        Active = profile.Active
                    });
                }

                return profileDtos;
            }
        }

        public static async Task<List<ProfilePreferenceDto>> GetProfilePreferenceDtos(int profileId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                return await db.ProfilePreferences.Where(x => x.ProfileId == profileId && x.Active).Select(x => new ProfilePreferenceDto()
                {
                    DayOfWeek = x.DayOfWeek,
                    Hour = x.Hour,
                    Minute = x.Minute
                }).ToListAsync();
            }
        }
    }
}
