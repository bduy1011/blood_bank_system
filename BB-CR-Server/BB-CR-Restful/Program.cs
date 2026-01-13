using BB.CR.Providers;
using BB.CR.Rest.Extensions;
using BB.CR.Rest.MessageQueues;
using BB.CR.Rest.Profiles;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.WriteIndented = true;
    //o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    option.EnableAnnotations();
    option.OperationFilter<CustomHeaderGen>();
});

string jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string?>() ?? string.Empty;
string jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string?>() ?? string.Empty;

Settings.ConnectionString = builder.Configuration.GetConnectionString("ChoRayConnection") ?? string.Empty;
Settings.KeySecurity = builder.Configuration.GetSection("KeySecurity").Get<string?>() ?? string.Empty;

Settings.PathTemp = builder.Configuration.GetSection("Upload:PathTemp").Get<string?>() ?? string.Empty;
Settings.PathActual = builder.Configuration.GetSection("Upload:PathActual").Get<string?>() ?? string.Empty;

Settings.Hospital = builder.Configuration.GetSection("HospitalInformation").Get<HospitalInformation>();

// Load device Kiosk
Settings.KioskDevices = builder.Configuration.GetSection("KioskDevices").Get<List<string>?>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.SingletonReg();

#region Redis cache
// Configuration read redis cache
builder.Services.AddStackExchangeRedisCache(option =>
{
    var connection = builder.Configuration["Redis:CacheServer"];
    option.Configuration = connection;
});
#endregion

//builder.Services.AddCustomPolicy(builder.Configuration);

#region RabbitMQ
RabbitMQSetting.UserName = builder.Configuration["RabbitMQ:Username"];
RabbitMQSetting.Password = builder.Configuration["RabbitMQ:Password"];
RabbitMQSetting.HostName = builder.Configuration["RabbitMQ:Hostname"];
//builder.Services.AddMassTransit(o =>
//{
//    o.UsingRabbitMq((context, configure) =>
//    {
//        //configure.Host(builder.Configuration[""], "/", host =>
//        //{
//        //});
//    });
//});
#endregion

builder.Services.AddMapster();
BaseMapping.ExecuteAllMapping();

#region Configure OTP
Settings.BreakGenerate = builder.Configuration.GetSection("OTP:BreakGenerate").Get<bool>();
Settings.ExpireTimeOtp = builder.Configuration.GetSection("OTP:ExpireTimeOtp").Get<int>();
#endregion

Settings.DeleteAccount = builder.Configuration.GetSection("Settings:DeleteAccount").Get<bool>();
Settings.DelayTime = builder.Configuration.GetSection("Settings:DelayTime").Get<int>();

#region Compress data
builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = System.IO.Compression.CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = System.IO.Compression.CompressionLevel.SmallestSize);

builder.Services.AddResponseCompression(o =>
{
    o.EnableForHttps = false;
    o.Providers.Add<BrotliCompressionProvider>();
    o.Providers.Add<GzipCompressionProvider>();
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()
    /*|| app.Environment.IsProduction()*/)
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BB-CR-API v1"));
}

app.UseHttpsRedirection();

//app.UseCustomPolicy(builder.Configuration);

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenValidationMiddleware>();

app.UseResponseCompression();

app.MapControllers();

app.Run();
