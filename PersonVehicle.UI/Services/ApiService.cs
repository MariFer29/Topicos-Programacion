using PersonVehicle.Model;
using PersonVehicle.UI.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Person = PersonVehicle.UI.Models.Person;
using Vehicle = PersonVehicle.UI.Models.Vehicle;

namespace PersonVehicle.UI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PersonVehicleApi");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        // Personas
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var response = await _httpClient.GetAsync("api/persons");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Person>>(json, _jsonOptions) ?? new List<Person>();
            }
            return new List<Person>();
        }

        public async Task<Person?> GetPersonByIdentificationAsync(int identification)
        {
            var response = await _httpClient.GetAsync($"api/persons/by-identification/{identification}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Person>(json, _jsonOptions);
            }
            return null;
        }

        public async Task<bool> CreatePersonAsync(Person person, int identification)
        {
            var json = JsonSerializer.Serialize(person, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/persons", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePersonAsync(int identification, Person person)
        {
            var json = JsonSerializer.Serialize(person, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/persons/{identification}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarPersonAsync(int identification)
        {
            var json = JsonSerializer.Serialize(identification, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.DeleteAsync($"api/persons/{identification}");
            return response.IsSuccessStatusCode;
        }


        // Vehículos
        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            var response = await _httpClient.GetAsync("api/vehicles");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Vehicle>>(json, _jsonOptions) ?? new List<Vehicle>();
            }
            return new List<Vehicle>();
        }

        public async Task<List<Vehicle>> GetVehiclesByOwnerAsync(string identification)
        {
            var response = await _httpClient.GetAsync($"api/vehicles/by-owner/{identification}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Vehicle>>(json, _jsonOptions) ?? new List<Vehicle>();
            }
            return new List<Vehicle>();
        }

        public async Task<bool> CreateVehicleAsync(Vehicle vehicle)
        {
            var json = JsonSerializer.Serialize(vehicle, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/vehicles", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateVehicleAsync(string plate, Vehicle vehicle)
        {
            var json = JsonSerializer.Serialize(vehicle, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/vehicles/{plate}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarVehicleAsync(string plate)
        {
            var json = JsonSerializer.Serialize(plate, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.DeleteAsync($"api/persons/{plate}");
            return response.IsSuccessStatusCode;
        }

    }
}