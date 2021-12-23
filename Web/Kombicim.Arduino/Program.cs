using Kombicim.APIShared.Middlewares;
using Kombicim.Data;
using Kombicim.Data.Repository;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson((jsonOptions) =>
{
    jsonOptions.UseMemberCasing();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicAuth", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<KombicimDataContext>();
builder.Services.AddScoped<ApiUserRepository>();
builder.Services.AddScoped<CombiLogRepository>();
builder.Services.AddScoped<DeviceRepository>();
builder.Services.AddScoped<LocationRepository>();
builder.Services.AddScoped<MinTemperatureRepository>();
builder.Services.AddScoped<ProfileRepository>();
builder.Services.AddScoped<SettingRepository>();
builder.Services.AddScoped<StateRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<WeatherRepository>();

var app = builder.Build();

var logger = app.Services.GetService<ILogger<Program>>();
logger.LogWarning($"Environment: {app.Environment.EnvironmentName}  | IsDevelopment -> {app.Environment.IsDevelopment()}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseMiddleware<BasicAuthMiddleware>();
app.MapControllers();

app.Run();
