
using System.Net.Http.Headers;
using System.Net.Http;
using Thermostat;
using Thermostat.Data;
using Thermostat.Models;
using Thermostat.Services;
using Thermostat.Services.Interfaces;

namespace Team03_SmartHome
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddSingleton<IThermostatService, ThermostatService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<Startup>();

            var app = builder.Build();

            Startup startupService = app.Services.GetRequiredService<Startup>();

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
