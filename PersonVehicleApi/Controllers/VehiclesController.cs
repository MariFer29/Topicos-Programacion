using Microsoft.AspNetCore.Mvc;
using PersonVehicle.BL;
using PersonVehicle.Model;
using PersonVehicle.Model.DTO;

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
        [HttpGet("{placa}")]
        public async Task<IActionResult> GetByPlate(string placa)
        {
            try
            {
                var result = await _advehicleRepository.ObtengaListaVehiclePlateAsync(placa);
                return Ok(result);
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

