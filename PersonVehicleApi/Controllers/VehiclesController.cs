using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.AspNetCore.Mvc;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/ServicioDePersonasVehiculos/Vehiculos/")]
    public class VehiclesController : ControllerBase
    {
        private readonly IAdministradorDeVehicles _advehicleRepository; // Servicio de lógica de negocio

        // Inyección del servicio BL
        public VehiclesController(IAdministradorDeVehicles advehicleRepository)
        {
            _advehicleRepository = advehicleRepository;
        }

        // GET:  — Obtiene todos los vehículos
        [HttpGet("ObtengaListaDeVehiculos")]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _advehicleRepository.ObtengaListaVehiclesAsync();
            return Ok(vehicles);
        }

        // GET: 
        // Obtiene la persona propietaria mediante la placa del vehículo
        [HttpGet("ObtengaListaDeVehiculoPorPlaca/placa={plate}")]
        public async Task<IActionResult> GetOwnerByPlate(string plate)
        {
            try
            {
                var result = await _advehicleRepository.ObtengaListaVehiclePlateAsync(plate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }

        // POST: 
        [HttpPost("AgregueNuevoVehiculo")]
        public async Task<IActionResult> CreateVehicle([FromBody] Vehicles vehicle)
        {
            try
            {
                var result = await _advehicleRepository.AgregueVehicleAsync(vehicle);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }

        // PUT: 
        // Actualiza datos del vehículo mediante su placa
        [HttpPut("ModifiqueElVehiculo/owner/placa={placa}")]
        public async Task<IActionResult> UpdateVehicle(string placa, [FromBody] Vehicles vehicle)
        {
            try
            {
                var result = await _advehicleRepository.ActualizarVehicleAsync(placa, vehicle);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }
        
        // Cambia el dueño del vehículo
        [HttpPut("ModifiquePropietario/owner/placa={placa}")]
        public async Task<IActionResult> UpdateOwner(string placa, [FromBody] Owner owner)
        {
            try
            {
                var result = await _advehicleRepository.ActualizarOwnerVehicleAsync(placa, owner);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }

        // DELETE: 
        // Elimina un vehículo por su placa
        [HttpDelete("ElimineElVehiculo/plate={plate}")]
        public async Task<IActionResult> DeleteVehicle(string plate)
        {
            try
            {
                var result = await _advehicleRepository.EliminarVehicleAsync(plate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }
    }
}
