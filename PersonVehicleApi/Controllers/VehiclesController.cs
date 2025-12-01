using Microsoft.AspNetCore.Mvc;
using PersonVehicle.BL;
using PersonVehicle.Model;
using PersonVehicle.Model.DTO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/vehiculos")]
    public class VehiclesController : ControllerBase
    {
        private readonly IAdministradorDeVehicles _advehicleRepository;

        public VehiclesController(IAdministradorDeVehicles advehicleRepository)
        {
            _advehicleRepository = advehicleRepository;
        }

        // GET: api/vehiculos
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _advehicleRepository.ObtengaListaVehiclesAsync();
            return Ok(vehicles);
        }

        // GET: api/vehiculos/{placa}
        // MODIFICADO - Usa serialización personalizada para incluir Owner y Person
        [HttpGet("{placa}")]
        public async Task<IActionResult> GetByPlate(string placa)
        {
            try
            {
                var result = await _advehicleRepository.ObtengaListaVehiclePlateAsync(placa);
                
                // Crear un DTO para la respuesta que no tenga [JsonIgnore]
                var response = new
                {
                    plate = result.Plate,
                    make = result.Make,
                    model = result.Model,
                    year = result.Year,
                    personIdentification = result.PersonIdentification,
                    owner = result.Owner != null ? new
                    {
                        ownerIdentification = result.Owner.OwnerIdentification,
                        person = result.Owner.Person != null ? new
                        {
                            identification = result.Owner.Person.Identification,
                            firstName = result.Owner.Person.FirstName,
                            lastName = result.Owner.Person.LastName,
                            email = result.Owner.Person.Email,
                            phone = result.Owner.Person.Phone,
                            salario = result.Owner.Person.Salario
                        } : null
                    } : null
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/vehiculos
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] Vehicles vehicle)
        {
            try
            {
                var result = await _advehicleRepository.AgregueVehicleAsync(vehicle);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/vehiculos/{placa}
        [HttpPut("{placa}")]
        public async Task<IActionResult> UpdateVehicle(string placa, [FromBody] VehicleUpdateDto vehicleDto)
        {
            try
            {
                var result = await _advehicleRepository.ActualizarVehicleAsync(placa, vehicleDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/vehiculos/{placa}/propietario
        [HttpPut("{placa}/propietario")]
        public async Task<IActionResult> UpdateOwner(string placa, [FromBody] Owner owner)
        {
            try
            {
                var result = await _advehicleRepository.ActualizarOwnerVehicleAsync(placa, owner);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/vehiculos/{placa}
        [HttpDelete("{placa}")]
        public async Task<IActionResult> DeleteVehicle(string placa)
        {
            try
            {
                var result = await _advehicleRepository.EliminarVehicleAsync(placa);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

