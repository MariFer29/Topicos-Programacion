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
        private readonly AppDbContext _db;
        public VehiclesController(AppDbContext db) { _db = db; }

        // GET: api/vehicles
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _db.Vehicles.Include(v => v.Owner).AsNoTracking().ToListAsync();
            return Ok(vehicles);
        }

        // GET: api/vehicles/by-owner/{identification}
        [HttpGet("by-owner/{identification}")]
        public async Task<IActionResult> GetByOwner(string identification)
        {
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);
            if (owner == null) return NotFound("Owner not found");

            var vehicles = await _db.Vehicles.Where(v => v.OwnerId == owner.Id).AsNoTracking().ToListAsync();
            return Ok(vehicles);
        }

        // POST: api/vehicles
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleDto dto)
        {
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == dto.OwnerIdentification);
            if (owner == null) return BadRequest("Owner (identification) not found");
            if (await _db.Vehicles.AnyAsync(v => v.Plate == dto.Plate))
                return Conflict("Vehicle with this plate already exists");

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
        [HttpPut("{plate}")]
        public async Task<IActionResult> UpdateVehicle(string plate, [FromBody] UpdateVehicleDto dto)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);
            if (vehicle == null) return NotFound();

            vehicle.Make = dto.Make ?? vehicle.Make;
            vehicle.Model = dto.Model ?? vehicle.Model;
            vehicle.Year = dto.Year ?? vehicle.Year;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
