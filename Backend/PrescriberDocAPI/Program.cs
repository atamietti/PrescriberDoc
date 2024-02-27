
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PrescriberDocAPI.Patients.Domain;
using PrescriberDocAPI.Patients.Infrastructure;
using PrescriberDocAPI.Shared.Domain;
using PrescriberDocAPI.UserManagement.Domain.UserAggregate;
using System.Text;

namespace PrescriberDocAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
            .Build();
            var userConfig = new UserConfig
            {
                IssuerSigningKey = configuration["PrecriberDocConfig:IssuerSigningKey"] ?? string.Empty,
                ConnectionStrinfg = configuration["PrecriberDocConfig:MongoDBConnectionString"] ?? string.Empty,
                DatabaseName = configuration["PrecriberDocConfig:DatabaseName"] ?? string.Empty
            };

            builder.Services.AddSingleton(userConfig);
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            var mongDbIdentityConfig = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = userConfig.ConnectionStrinfg,
                    DatabaseName = userConfig.DatabaseName,

                },
                IdentityOptionsAction = options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireLowercase = false;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.User.RequireUniqueEmail = true;
                }

            };

            builder.Services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongDbIdentityConfig)
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["PrecriberDocConfig:ValidIssuer"],
                    ValidAudience = configuration["PrecriberDocConfig:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(userConfig.IssuerSigningKey)),
                    ClockSkew = TimeSpan.Zero
                };

            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
             
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowAnyOrigin()
                );

            app.MapControllers();

            var mapMethod = typeof(CrudBase).GetMethod(nameof(CrudBase.MapType));
            foreach (var type in CrudBase.RegisteredTypes)
                mapMethod?.MakeGenericMethod(type).Invoke(null, new object?[] { app });


            app.Run();
        }
    }
}