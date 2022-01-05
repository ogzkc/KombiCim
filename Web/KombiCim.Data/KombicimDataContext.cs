using Kombicim.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kombicim.Data
{
    public class KombicimDataContext : DbContext
    {
        public KombicimDataContext()
        {

        }
        public KombicimDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApiTokenEntity> ApiTokens { get; set; }
        public DbSet<ApiUserEntity> ApiUsers { get; set; }
        public DbSet<CombiLogEntity> CombiLogs { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<MinTemperatureEntity> MinTemperatures { get; set; }
        public DbSet<ProfileEntity> Profiles { get; set; }
        public DbSet<ProfilePreferenceEntity> ProfilePreferences { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<CombiStateEntity> States { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<WeatherEntity> Weathers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=xxxxxxxxxxxx;Database=Kombicim;User ID=kombicim;Password=xxxxxxxxxxxxxx;"); // TODO: read connection string from appSettings.json
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.UseCollation("SQL_Latin1_General_CP1254_CI_AS");  -> its for turkish support

            modelBuilder.Entity<ApiTokenEntity>(entity =>
            {
                entity.ToTable("ApiToken");

                entity.Property(e => e.CreatedAt).HasColumnType("smalldatetime");

                entity.Property(e => e.Token)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ApiTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ApiToken_User");
            });

            modelBuilder.Entity<ApiUserEntity>(entity =>
            {
                entity.ToTable("ApiUser");

                entity.Property(e => e.Password).HasMaxLength(500);
                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<CombiLogEntity>(entity =>
            {
                entity.ToTable("CombiLog");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.CombiLogs)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CombiLog_Device");
            });

            modelBuilder.Entity<DeviceEntity>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Id)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CenterDeviceId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Device_User");
            });


            modelBuilder.Entity<LocationEntity>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Location_Device");
            });

            modelBuilder.Entity<MinTemperatureEntity>(entity =>
            {
                entity.ToTable("MinTemperature");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.MinTemperatures)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_MinTemperature_Location");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.MinTemperatures)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_MinTemperature_Profile");
            });

            modelBuilder.Entity<ProfileEntity>(entity =>
            {
                entity.ToTable("Profile");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Profile_User");
            });
            modelBuilder.Entity<ProfilePreferenceEntity>(entity =>
            {
                entity.ToTable("ProfilePreference");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.ProfilePreferences)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProfilePreference_Device");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfilePreferences)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProfilePreference_Profile");
            });

            modelBuilder.Entity<SettingEntity>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.Id)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Settings)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Setting_Device");
            });

            modelBuilder.Entity<CombiStateEntity>(entity =>
            {
                entity.ToTable("CombiState");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.CombiStates)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CombiState_Device");
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.EmailAddress).HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(500);
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_Device");
            });

            modelBuilder.Entity<WeatherEntity>(entity =>
            {
                entity.ToTable("Weather");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Weathers)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weather_Location");
            });
        }
    }
}
