using KombiCim.Data.Models.Arduino;
using KombiCim.Data.Models;
using KombiCim.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KombiCim.Data.Models.Mobile.Dtos;

namespace KombiCim.Data.Repository
{
    public class WeatherRepository : BaseRepository
    {
        public static async Task<List<WeatherData>> GetAll(int userId, string deviceId, int lastHours = 24, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var dataList = new List<WeatherData>();
                var lastDate = Now.AddHours(-lastHours);

                var activeProfile = await ProfileRepository.GetActive(userId, db);

                var locationDtos = await LocationRepository.GetLocationDtos(deviceId, activeProfile.Id, db);
                foreach (var locationDto in locationDtos)
                {
                    var weatherData = new WeatherData();
                    weatherData.Location = locationDto;
                    weatherData.WeatherList = await db.Weathers
                        .Where(x => x.LocationId == locationDto.Id && x.Date > lastDate)
                        .OrderByDescending(x => x.Id).Select(x => new WeatherDto()
                        {
                            Temperature = x.Temperature,
                            Humidity = x.Humidity,
                            Date = x.Date
                        }).ToListAsync();

                    dataList.Add(weatherData);
                }


                return dataList;
            }
        }

        public static async Task<Weather> GetLastHour(int locationId, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var lastDay = Now.AddHours(-1);
                var weather = await db.Weathers.Where(x => x.LocationId == locationId && x.Date > lastDay).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                return weather;
            }
        }

        public static async Task<Weather> Post(string deviceId, double temperature, double humidity, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;
                var locationId = (await LocationRepository.Get(deviceId, db)).Id;
                return await Post(locationId, temperature, humidity, db);
            }
        }

        public static async Task<Weather> Post(int locationId, double temperature, double humidity, KombiCimEntities db_ = null)
        {
            using (var dbHelper = new DbHelper(db_))
            {
                var db = dbHelper.Db;

                var weather = new Weather()
                {
                    LocationId = locationId,
                    Temperature = temperature,
                    Humidity = humidity,
                    Date = Now
                };
                db.Weathers.Add(weather);
                await db.SaveChangesAsync();

                return weather;
            }
        }
    }
}
