using MainUnit.Models.Thermostat;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

        public async Task<ThermostatWithURL> UpdateThermostatAsync(ThermostatWithURL thermostatWithURL)
        {
            //TODO Change API Route to route of thermostat
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
            $"api/thermostat/{thermostatWithURL.Id}", thermostatWithURL);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated Thermostat from the response body.
            thermostatWithURL = await response.Content.ReadAsAsync<ThermostatWithURL>();
            return thermostatWithURL;
        }
    }
}
