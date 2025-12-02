using Newtonsoft.Json;
using PersonVehicle.UI.Models;
using System.Text;

namespace PersonVehicle.UI.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerSettings _jsonSettings;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        // ==================== PERSONAS ====================

        public async Task<bool> AgregarPersonaAsync(Persons person)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var json = JsonConvert.SerializeObject(person, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/persons", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Persons>> ObtenerListaPersonasAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var response = await client.GetAsync("api/persons");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<Persons>>(result, _jsonSettings);
                    return lista ?? new List<Persons>();
                }
                
                return new List<Persons>();
            }
            catch
            {
                return new List<Persons>();
            }
        }

        public async Task<Persons?> ObtenerListaPersonasPorIdentificacionAsync(int identification)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var response = await client.GetAsync($"api/persons/{identification}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var person = JsonConvert.DeserializeObject<Persons>(result, _jsonSettings);
                    return person;
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Persons?> GetPersonByIdentificationAsync(int identification)
        {
            return await ObtenerListaPersonasPorIdentificacionAsync(identification);
        }

        public async Task<Persons?> EditarPersonaAsync(int identification, Persons persona)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var response = await client.GetAsync($"api/persons/{identification}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var person = JsonConvert.DeserializeObject<Persons>(result, _jsonSettings);
                    return person;
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdatePersonAsync(int identification, Persons person)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                
                // Crear DTO para actualización
                var updateDto = new
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Email = person.Email,
                    Phone = person.Phone,
                    Salario = person.Salario
                };
                
                var json = JsonConvert.SerializeObject(updateDto, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/persons/{identification}", content);
                
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarPersonaAsync(int identification)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var response = await client.DeleteAsync($"api/persons/{identification}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ==================== VEHICULOS ====================

        public async Task<bool> AgregueNuevoVehiculo(Vehicles vehicle)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var json = JsonConvert.SerializeObject(vehicle, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/vehiculos", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Vehicles>> GetAllVehiclesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var response = await client.GetAsync("api/vehiculos");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<Vehicles>>(result, _jsonSettings);
                    return lista ?? new List<Vehicles>();
                }
                
                return new List<Vehicles>();
            }
            catch
            {
                return new List<Vehicles>();
            }
        }

        public async Task<List<Vehicles>> ObtengaListaDeVehiculo()
        {
            return await GetAllVehiclesAsync();
        }

        public async Task<Vehicles?> ObtengaListaDeVehiculoPorPlaca(string plate)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var response = await client.GetAsync($"api/vehiculos/{plate}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var vehicle = JsonConvert.DeserializeObject<Vehicles>(result, _jsonSettings);
                    return vehicle;
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateVehicleAsync(string plate, Vehicles vehicle)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                
                // Crear DTO para actualización
                var updateDto = new
                {
                    Plate = vehicle.Plate,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    Year = vehicle.Year
                };
                
                var json = JsonConvert.SerializeObject(updateDto, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/vehiculos/{plate}", content);
                
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditarVehiculoAsync(string placa, Vehicles vehicle)
        {
            return await UpdateVehicleAsync(placa, vehicle);
        }

        public async Task<(bool success, string message)> EditarVehiculoPropietarioAsync(string placa, int ownerIdentification)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                
                var ownerDto = new
                {
                    OwnerIdentification = ownerIdentification
                };
                
                var json = JsonConvert.SerializeObject(ownerDto, _jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/vehiculos/{placa}/propietario", content);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    // Limpiar el mensaje de las comillas si viene como string JSON
                    var message = responseContent.Trim('"');
                    return (true, message);
                }
                else
                {
                    // Si hay error, intentar obtener el mensaje de error
                    var errorMessage = responseContent.Trim('"');
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<bool> EliminarVehiculoAsync(string plate)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PersonVehicleApi");
                var response = await client.DeleteAsync($"api/vehiculos/{plate}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}