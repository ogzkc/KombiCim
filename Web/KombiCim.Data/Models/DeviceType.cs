using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombicim.Data.Models
{
    public class DeviceType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public const string CENTER_NAME = "center";
        public const string THERMOMETER_NAME = "thermometer";

        public const int CENTER_ID = 1;
        public const int THERMOMETER_ID = 2;
        public static List<DeviceType> All => new List<DeviceType> { new DeviceType() { Id = CENTER_ID, Name = CENTER_NAME }, new DeviceType() { Id = THERMOMETER_ID, Name = THERMOMETER_NAME } };
        public static string GetName(int deviceTypeId) => All.Where(x => x.Id == deviceTypeId).SingleOrDefault()?.Name;
    }
}
