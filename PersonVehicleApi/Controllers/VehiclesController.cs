using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;
using PersonVehicleApi.Model;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly AppDbContext _db; // Contexto de base de datos

        public VehiclesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/vehicles — Obtiene todos los vehículos
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _db.Vehicles
                .Include(v => v.Owner) // Incluye información del dueño
                .AsNoTracking()
                .ToListAsync();

            return Ok(vehicles);
        }

        // GET: api/vehicles/by-owner/{identification}
        // Busca vehículos según la identificación del dueño
        [HttpGet("by-owner/{identification}")]
        public async Task<IActionResult> GetByOwner(string identification)
        {
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);
            if (owner == null) return NotFound("Owner not found");

            var vehicles = await _db.Vehicles
                .Where(v => v.OwnerId == owner.Id)
                .AsNoTracking()
                .ToListAsync();

            return Ok(vehicles);
        }

        // POST: api/vehicles — Crear un nuevo vehículo
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleDto dto)
        {
            // Verifica que exista el dueño
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == dto.OwnerIdentification);
            if (owner == null) return BadRequest("Owner (identification) not found");

            // Verifica si la placa ya está registrada
            if (await _db.Vehicles.AnyAsync(v => v.Plate == dto.Plate))
                return Conflict("Vehicle with this plate already exists");

            // Crear nuevo vehículo
            var vehicle = new Vehicle
            {
                Plate = dto.Plate,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                OwnerId = owner.Id
            };

            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicles), new { id = vehicle.Id }, vehicle);
        }

        // PUT: api/vehicles/{plate}
        // Actualiza datos del vehículo
        [HttpPut("{plate}")]
        public async Task<IActionResult> UpdateVehicle(string plate, [FromBody] UpdateVehicleDto dto)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);
            if (vehicle == null) return NotFound();

            // Actualiza sólo los campos enviados
            vehicle.Make = dto.Make ?? vehicle.Make;
            vehicle.Model = dto.Model ?? vehicle.Model;
            vehicle.Year = dto.Year ?? vehicle.Year;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
