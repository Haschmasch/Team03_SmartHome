using MainUnit.Models.Settings;
using MainUnit.Services;
using MainUnit.Services.Interfaces;

namespace MainUnit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //This gets the MongoDbSettings from the environement variables of the docker-compose file
            //unless they are specified in the appsettings.json. The expected syntax in the docker compose:
            //MongoDbSettings__ConnectionURI = xxx
            builder.Services.Configure<MongoDbSettings>(
            builder.Configuration.GetSection("MongoDbSettings"));

            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IThermostatService, ThermostatService>();
            builder.Services.AddScoped<IRoomTemperatureService, RoomTemperatureService>();
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
