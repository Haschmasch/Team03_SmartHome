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
            _httpClient.BaseAddress = new Uri(uri);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> UpdateThermostatAsync(ThermostatWithURL thermostatWithURL)
        {
            //TODO Change API Route to route of thermostat
            HttpResponseMessage response = await _httpClient.PostAsync(
            $"api/thermostat?temperature={thermostatWithURL.Temperature}", null);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<bool>();
        }
    }
}
