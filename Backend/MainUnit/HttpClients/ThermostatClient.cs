using MainUnit.Models.Thermostat;
using System.Net.Http.Headers;

namespace MainUnit.HttpClients
{
    public class ThermostatClient
    {
        private readonly HttpClient _httpClient;

        public ThermostatClient(string uri)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"http://{name}:8080");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> UpdateThermostatAsync(Thermostat thermostat)
        {
            //TODO Change API Route to route of thermostat
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
            $"api/thermostat/{thermostatWithURL.Id}", thermostatWithURL);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsAsync<bool>();
        }
    }
}
