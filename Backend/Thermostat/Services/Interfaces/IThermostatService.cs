﻿using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Thermostat.Models;

namespace Thermostat.Services.Interfaces
{
    public interface IThermostatService
    {
        /// <summary>   Updates the temperature described by temperature. </summary>
        ///
        /// <param name="temperature">  The temperature. </param>
        ///
        /// <returns>   A Result&lt;bool&gt; </returns>

        public Result<bool> UpdateTemperature(float temperature);
    }
}
