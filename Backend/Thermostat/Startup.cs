using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Thermostat.Data;
using Thermostat.Models;
using Thermostat.Models.Auth;

namespace Thermostat
{
    public class Startup
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly Uri _mainUnitUri = new("http://host.docker.internal:8085/", UriKind.Absolute);
        private readonly Uri _requestUri = new("api/Thermostats", UriKind.Relative);

        public Startup(IHostApplicationLifetime applicationLifetime)
        {
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

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",  JsonConvert.DeserializeObject<Token>(bearerToken)?.token);
        }

        private async void RegisterAtMainUnit()
        {
            Console.WriteLine("Registering at main unit...");
            
            HttpResponseMessage response = await _httpClient.PostAsync("api/Thermostats?name=" + Environment.GetEnvironmentVariable("ThermostatID"), null);
            response.EnsureSuccessStatusCode();

            var thermostatResponse = await response.Content.ReadAsAsync<ThermostatObject>();

            Memory.Id = thermostatResponse.Id;

            Console.WriteLine("Registered successfully");
        }
    }
}
