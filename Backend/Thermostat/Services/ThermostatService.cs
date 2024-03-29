﻿using LanguageExt.Common;
using Thermostat.Data;
using Thermostat.Services.Interfaces;

namespace Thermostat.Services
{
    public class ThermostatService : IThermostatService
    {
        /// <inheritdoc />
        public Result<bool> UpdateTemperature(float? temperature)
        {
            if (temperature.HasValue && !float.IsNaN(temperature.Value))
            {
                Memory.Temperature = temperature.Value;

                Console.WriteLine($"Temperature changed to {Memory.Temperature:F2}°C");

                return new Result<bool>(true);
            }

            return new Result<bool>(false);
        }
    }
}
