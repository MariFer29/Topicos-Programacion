using Microsoft.AspNetCore.Mvc;
using PersonVehicleApi.BL;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly VehiclesBL _bl; // Servicio de lógica de negocio

        // Inyección del servicio BL
        public VehiclesController(VehiclesBL bl)
        {
            _bl = bl;
        }

        // GET: api/vehicles — Obtiene todos los vehículos
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _bl.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        // GET: api/vehicles/by-owner/{identification}
        // Obtiene todos los vehículos de un propietario por su identificación
        [HttpGet("by-owner/{identification}")]
        public async Task<IActionResult> GetVehiclesByOwner(string identification)
        {
            var result = await _bl.GetByOwnerAsync(identification);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Vehicles);
        }

        // GET: api/vehicles/owner-by-plate/{plate}
        // Obtiene la persona propietaria mediante la placa del vehículo
        [HttpGet("owner-by-plate/{plate}")]
        public async Task<IActionResult> GetOwnerByPlate(string plate)
        {
            var result = await _bl.GetOwnerByPlateAsync(plate);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Owner);
        }

        // POST: api/vehicles — Crear un nuevo vehículo
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleDto dto)
        {
            var result = await _bl.CreateVehicleAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetVehicles), new { id = result.CreatedVehicle!.Id }, result.CreatedVehicle);
        }

        // PUT: api/vehicles/{plate}
        // Actualiza datos del vehículo mediante su placa
        [HttpPut("{plate}")]
        public async Task<IActionResult> UpdateVehicle(string plate, [FromBody] UpdateVehicleDto dto)
        {
            var result = await _bl.UpdateVehicleAsync(plate, dto);

            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }

        // PUT: api/vehicles/{plate}/change-owner
        // Cambia el dueño del vehículo
        [HttpPut("{plate}/change-owner")]
        public async Task<IActionResult> UpdateOwner(string plate, [FromBody] UpdateVehicleOwnerDto dto)
        {
            var result = await _bl.UpdateVehicleOwnerAsync(plate, dto.NewOwnerIdentification);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        // DELETE: api/vehicles/{plate}
        // Elimina un vehículo por su placa
        [HttpDelete("{plate}")]
        public async Task<IActionResult> DeleteVehicle(string plate)
        {
            var result = await _bl.DeleteVehicleAsync(plate);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);
        }
    }
}
