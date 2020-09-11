using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Project designed db-first style. You can take code-first models from here.
/// 
/// WARNING
/// MinOperator table is overdesigned
/// DayOfWeek,Hour,Minute,DisabledAt Columns in MinTemperature table are overdesigned
/// ------------------
/// </summary>
namespace KombiCim.Data.Models.CodeFirst
{
    public class ApiAuthType
    {
//        Id     Name
//         1	IoTDevice
//         2	MobileApp

        public ApiAuthType()
        {
            this.ApiUsers = new List<ApiUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<ApiUser> ApiUsers { get; set; }
    }

    public class ApiToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }

    public class ApiUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int ApiAuthTypeId { get; set; }
        public bool Active { get; set; }

        public ApiAuthType ApiAuthType { get; set; }
    }

    public class CombiLog
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public bool State { get; set; }
        public DateTime Date { get; set; }

        public Device Device { get; set; }
    }

    public class Device
    {
        public Device()
        {
            this.CombiLogs = new List<CombiLog>();
            this.Locations = new List<Location>();
            this.MinOperators = new List<MinOperator>();
            this.ProfilePreferences = new List<ProfilePreference>();
            this.Settings = new List<Setting>();
            this.States = new List<State>();
            this.Users = new List<User>();
        }

        public string Id { get; set; }
        public string CenterDeviceId { get; set; }
        public int TypeId { get; set; }
        public int? OwnerUserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public DeviceType DeviceType { get; set; }
        public User User { get; set; }
        public List<CombiLog> CombiLogs { get; set; }
        public List<Location> Locations { get; set; }
        public List<MinOperator> MinOperators { get; set; }
        public List<ProfilePreference> ProfilePreferences { get; set; }
        public List<Setting> Settings { get; set; }
        public List<State> States { get; set; }
        public List<User> Users { get; set; }
    }

    public class DeviceType
    {
        public DeviceType()
        {
            this.Devices = new List<Device>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<Device> Devices { get; set; }
    }

    public class Location
    {
        public Location()
        {
            this.MinTemperatures = new List<MinTemperature>();
            this.Weathers = new List<Weather>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }

        public Device Device { get; set; }
        public List<MinTemperature> MinTemperatures { get; set; }
        public List<Weather> Weathers { get; set; }
    }

    public class MinOperator
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Device Device { get; set; }
    }

    public class MinTemperature
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int LocationId { get; set; }
        public int ProfileId { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public Location Location { get; set; }
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        public Profile()
        {
            this.MinTemperatures = new List<MinTemperature>();
            this.ProfilePreferences = new List<ProfilePreference>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }

        public ProfileType ProfileType { get; set; }
        public User User { get; set; }
        public List<MinTemperature> MinTemperatures { get; set; }
        public List<ProfilePreference> ProfilePreferences { get; set; }
    }

    public class ProfilePreference
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string DeviceId { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
        public int? ActiveProfileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }

        public Profile Profile { get; set; }
        public Device Device { get; set; }
    }

    public class ProfileType
    {
        public ProfileType()
        {
            this.Profiles = new List<Profile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ServerBased { get; set; }

        public List<Profile> Profiles { get; set; }
    }

    public class Setting
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public Device Device { get; set; }
    }

    public class State
    {
        public int Id { get; set; }
        public bool Value { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }

        public Device Device { get; set; }
    }

    public class User
    {
        public User()
        {
            this.ApiTokens = new List<ApiToken>();
            this.Devices = new List<Device>();
            this.Profiles = new List<Profile>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string OwnedDeviceId { get; set; }
        public bool Active { get; set; }

        public Device Device { get; set; }
        public List<ApiToken> ApiTokens { get; set; }
        public List<Device> Devices { get; set; }
        public List<Profile> Profiles { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }

        public Location Location { get; set; }
    }

    public class WeatherView
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string DeviceId { get; set; }
    }

}
