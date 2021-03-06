﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KombiCim.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KombiCimEntities : DbContext
    {
        public KombiCimEntities()
            : base("name=KombiCimEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ApiAuthType> ApiAuthTypes { get; set; }
        public virtual DbSet<ApiToken> ApiTokens { get; set; }
        public virtual DbSet<ApiUser> ApiUsers { get; set; }
        public virtual DbSet<CombiLog> CombiLogs { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MinOperator> MinOperators { get; set; }
        public virtual DbSet<MinTemperature> MinTemperatures { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<ProfilePreference> ProfilePreferences { get; set; }
        public virtual DbSet<ProfileType> ProfileTypes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Weather> Weathers { get; set; }
    }
}
