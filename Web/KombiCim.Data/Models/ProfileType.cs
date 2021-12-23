using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombicim.Data.Models
{
    public class ProfileType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ServerBased { get; set; }


        public const string MODE_AUTO_PROFILE = "auto_profile_1"; // Sabit sıcaklığa göre profil bazlı otomatik kombi kontrolü
        public const string MODE_AUTO_SERVER_PROFILE = "auto_server_profile_1"; // Sabit sıcaklıklara fakat çoklu konumlara göre profil bazlı ve sunucu merkezli otomatik kombi kontrolü

        public const string MODE_AUTO_PROFILE_SCHEDULED_1 = "auto_profile_scheduled_1"; // Haftanın 7 günü için farklı saatlerde farklı profiller ayarlanabilen otomatik kombi kontrolü
        public const string MODE_AUTO_PROFILE_SERVER_SCHEDULED_1 = "auto_server_profile_scheduled_1"; // Haftanın 7 günü için farklı konumlar için farklı saatlerde farklı profiller ayarlanabilen, sunucu merkezli otomatik kombi kontrolü

        public const string MODE_AUTO_SCHEDULED_1 = "auto_scheduled_1"; // Haftanın 7 günü için farklı saatlerde farklı sıcaklıklarla ayarlanabilen otomatik kombi kontrolü
        public const string MODE_AUTO_SERVER_SCHEDULED_1 = "auto_server_scheduled_1"; // Haftanın 7 günü farklı konumlar için farklı saatlerde farklı sıcaklıklar ayarlanabilen, sunucu merkezli otomatik kombi kontrolü

        public const string MODE_MANUAL = "manual_1"; // Kombiyi doğrudan açık yada kapalı konumda tutma


        public const int MODE_AUTO_PROFILE_ID = 3;
        public const int MODE_AUTO_SERVER_PROFILE_ID = 4;
        public const int MODE_AUTO_PROFILE_SCHEDULED_1_ID = 5;
        public const int MODE_AUTO_PROFILE_SERVER_SCHEDULED_1_ID = 6;
        public const int MODE_AUTO_SCHEDULED_1_ID = 7;
        public const int MODE_AUTO_SERVER_SCHEDULED_1_ID = 8;
        public const int MODE_MANUAL_ID = 10;

        public const string MODE_AUTO_PROFILE_DESC = "Sabit sıcaklığa göre profil bazlı otomatik kombi kontrolü";
        public const string MODE_AUTO_SERVER_PROFILE_DESC = "Sabit sıcaklıklara fakat çoklu konumlara göre profil bazlı ve sunucu merkezli otomatik kombi kontrolü";
        public const string MODE_AUTO_PROFILE_SCHEDULED_1_DESC = "Haftanın 7 günü için farklı saatlerde farklı profiller ayarlanabilen otomatik kombi kontrolü";
        public const string MODE_AUTO_PROFILE_SERVER_SCHEDULED_1_DESC = "Haftanın 7 günü için farklı konumlar için farklı saatlerde farklı profiller ayarlanabilen, sunucu merkezli otomatik kombi kontrolü";
        public const string MODE_AUTO_SCHEDULED_1_DESC = "Haftanın 7 günü için farklı saatlerde farklı sıcaklıklarla ayarlanabilen otomatik kombi kontrolü";
        public const string MODE_AUTO_SERVER_SCHEDULED_1_DESC = "Haftanın 7 günü farklı konumlar için farklı saatlerde farklı sıcaklıklar ayarlanabilen, sunucu merkezli otomatik kombi kontrolü";
        public const string MODE_MANUAL_DESC = "Kombiyi doğrudan açık ya da kapalı konumda tutma";

        public static List<ProfileType> All
        {
            get
            {
                var profileTypes = new List<ProfileType>();
                profileTypes.Add(new ProfileType()
                {
                    Id = MODE_AUTO_PROFILE_ID,
                    Name = MODE_AUTO_PROFILE,
                    Description = MODE_AUTO_PROFILE_DESC,
                    ServerBased = false
                });
                profileTypes.Add(new ProfileType()
                {
                    Id = MODE_AUTO_SERVER_PROFILE_ID,
                    Name = MODE_AUTO_SERVER_PROFILE,
                    Description = MODE_AUTO_SERVER_PROFILE_DESC,
                    ServerBased = true
                });

                profileTypes.Add(new ProfileType()
                {
                    Id = MODE_AUTO_PROFILE_SCHEDULED_1_ID,
                    Name = MODE_AUTO_PROFILE_SCHEDULED_1,
                    Description = MODE_AUTO_PROFILE_SCHEDULED_1_DESC,
                    ServerBased = false
                });

                profileTypes.Add(new ProfileType()
                {
                    Id = MODE_AUTO_PROFILE_SERVER_SCHEDULED_1_ID,
                    Name = MODE_AUTO_PROFILE_SERVER_SCHEDULED_1,
                    Description = MODE_AUTO_PROFILE_SERVER_SCHEDULED_1_DESC,
                    ServerBased = true
                });

                profileTypes.Add(new ProfileType()
                {
                    Id = MODE_AUTO_SCHEDULED_1_ID,
                    Name = MODE_AUTO_SCHEDULED_1,
                    Description = MODE_AUTO_SCHEDULED_1_DESC,
                    ServerBased = false
                });

                profileTypes.Add(new ProfileType()
                {
                    Id = MODE_AUTO_SERVER_SCHEDULED_1_ID,
                    Name = MODE_AUTO_SERVER_SCHEDULED_1,
                    Description = MODE_AUTO_SERVER_SCHEDULED_1_DESC,
                    ServerBased = true
                });

                profileTypes.Add(new ProfileType()
                {
                    Id = MODE_MANUAL_ID,
                    Name = MODE_MANUAL,
                    Description = MODE_MANUAL_DESC,
                    ServerBased = false
                });

                return profileTypes;
            }
        }

        public static string GetName(int profileTypeId) => All.Where(x => x.Id == profileTypeId).SingleOrDefault()?.Name;


    }
}
