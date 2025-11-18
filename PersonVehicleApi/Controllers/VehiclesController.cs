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
        // Obtiene vehículos mediante la identificación del dueño
        [HttpGet("by-owner/{identification}")]
        public async Task<IActionResult> GetByOwner(string identification)
        {
            var result = await _bl.GetByOwnerAsync(identification);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Vehicles);
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
    }
}
