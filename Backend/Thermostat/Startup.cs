using Newtonsoft.Json;
using System.Net.Http.Headers;
using Thermostat.Data;
using Thermostat.Models;
using Thermostat.Models.Auth;

namespace Thermostat
{
    public class Startup
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly Uri _mainUnitUri;

        public Startup(IHostApplicationLifetime applicationLifetime)
        {
            string envMainUnitURL = Environment.GetEnvironmentVariable("MainUnitURL")!;
            _mainUnitUri = new(envMainUnitURL, UriKind.Absolute);
            applicationLifetime.ApplicationStarted.Register(OnApplicationStarted);
        }

        public async void OnApplicationStarted()
        {
            _httpClient.BaseAddress = _mainUnitUri;

            await Authenticate();
            RegisterAtMainUnit();
        }

        private async Task Authenticate()
        {
            //Hardcoded because this will not change.
            var jsonContent = JsonContent.Create(new UserLogin("svc_thermostat", "E9;$t,hD:%_2-{!ksm46vz"));

            HttpResponseMessage response = await _httpClient.PostAsync("api/Auth/login", jsonContent);
            response.EnsureSuccessStatusCode();

            var bearerToken = await response.Content.ReadAsStringAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JsonConvert.DeserializeObject<Token>(bearerToken)?.token);
        }

        private async void RegisterAtMainUnit()
        {
            Console.WriteLine("Registering at main unit...");
            string url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")!;
            string containerName = Environment.GetEnvironmentVariable("ThermostatName")!;
            url = url.Replace("+", containerName);
            HttpResponseMessage response = await _httpClient.PostAsync("api/Thermostats?url=" + url, null);
            var thermostatResponse = await response.Content.ReadAsAsync<ThermostatObject>();

            Memory.Id = thermostatResponse.Id;

            Console.WriteLine("Registered successfully");
        }
    }
}
