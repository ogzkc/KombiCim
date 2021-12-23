using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombicim.Data.Models
{
    public class ApiAuthType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // 1 = IoTDevice, 2 = MobileApp

        public const string IOT_DEVICE_NAME = "IoTDevice";
        public const string MOBILE_APP_NAME = "MobileApp";

        public const int IOT_DEVICE_ID = 1;
        public const int MOBILE_APP_ID = 2;
        public static List<ApiAuthType> All => new List<ApiAuthType> { new ApiAuthType() { Id = IOT_DEVICE_ID, Name = IOT_DEVICE_NAME }, new ApiAuthType() { Id = MOBILE_APP_ID, Name = MOBILE_APP_NAME } };
        public static string GetName(int authTypeId) => All.Where(x => x.Id == authTypeId).SingleOrDefault()?.Name;
    }
}
