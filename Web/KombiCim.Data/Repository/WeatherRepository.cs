using Kombicim.Data.Utilities;
using Microsoft.EntityFrameworkCore;
using Kombicim.Data.Models.Mobile.Dtos;
using Kombicim.Data.Entities;

namespace Kombicim.Data.Repository
{
    public class WeatherRepository : BaseRepository
    {
        private readonly LocationRepository locationRepository;

        public WeatherRepository(KombicimDataContext kombiCimDataContext, LocationRepository locationRepository) : base(kombiCimDataContext)
        {
            this.locationRepository = locationRepository;
        }

        public async Task<List<WeatherData>> GetAll(int userId, string deviceId, int lastHours = 24)
        {

            var dataList = new List<WeatherData>();
            var lastDate = Now.AddHours(-lastHours);

            var activeProfile = await Db.Profiles.Where(x => x.UserId == userId && x.Active).SingleOrDefaultAsync();

            var locationDtos = await locationRepository.GetLocationDtos(deviceId, activeProfile.Id);
            foreach (var locationDto in locationDtos)
            {
                var weatherData = new WeatherData();
                weatherData.Location = locationDto;
                weatherData.WeatherList = await Db.Weathers
                    .Where(x => x.LocationId == locationDto.Id && x.CreatedAt > lastDate)
                    .OrderByDescending(x => x.Id).Select(x => new WeatherDto()
                    {
                        Temperature = x.Temperature,
                        Humidity = x.Humidity,
                        Date = x.CreatedAt
                    }).ToListAsync();

                dataList.Add(weatherData);
            }


            return dataList;
        }

        public async Task<List<WeatherEntity>> GetLastMinutes(int locationId, int minutes = 60)
        {
            var lastTime = Now.AddMinutes(-minutes);
            var weather = await Db.Weathers.Where(x => x.LocationId == locationId && x.CreatedAt > lastTime).OrderByDescending(x => x.Id).ToListAsync();
            return weather;
        }

        public async Task<WeatherEntity> Post(string deviceId, double temperature, double humidity)
        {
            var locationId = (await locationRepository.Get(deviceId)).Id;
            return await Post(locationId, temperature, humidity);
        }

        public async Task<WeatherEntity> Post(int locationId, double temperature, double humidity)
        {
            var weather = new WeatherEntity()
            {
                LocationId = locationId,
                Temperature = Math.Round(temperature, 1),
                Humidity = Math.Round(humidity, 1),
                CreatedAt = Now
            };
            Db.Weathers.Add(weather);
            await Db.SaveChangesAsync();

            return weather;
        }
    }
}
