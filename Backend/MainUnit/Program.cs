using System.Text;
using MainUnit.Models.Settings;
using MainUnit.Services;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MainUnit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLogging(builder =>
                builder.AddDebug()
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information)
            );
            // Add services to the container.
            //This gets the MongoDbSettings from the environement variables of the docker-compose file
            //unless they are specified in the appsettings.json. The expected syntax in the docker compose:
            //MongoDbSettings__ConnectionURI = xxx
            builder.Services.Configure<MongoDbSettings>(
            builder.Configuration.GetSection("MongoDbSettings"));

            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IThermostatService, ThermostatService>();
            builder.Services.AddScoped<IRoomTemperatureService, RoomTemperatureService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var key = Encoding.ASCII.GetBytes("LQKeZVE_x+-v{4zsnrPMwt76AJ#3,=R'<\"%W5h;g?yYF>!}@)c");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddAuthorization();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers().RequireAuthorization();

            app.Run();
        }
    }
}
