using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;
using PersonVehicleApi.Model;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PersonsController(AppDbContext db) { _db = db; }

        // GET: api/persons
        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _db.Persons.AsNoTracking().ToListAsync();
            return Ok(persons);
        }

        // GET: api/persons/by-identification/{identification}
        [HttpGet("by-identification/{identification}")]
        public async Task<IActionResult> GetByIdentification(string identification)
        {
            var person = await _db.Persons
                .Include(p => p.Vehicles)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identification == identification);

            if (person == null) return NotFound();
            return Ok(person);
        }

        // POST: api/persons
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Identification)) return BadRequest("Identification is required.");
            if (await _db.Persons.AnyAsync(p => p.Identification == dto.Identification))
                return Conflict("Person with this identification already exists.");

            var person = new Person
            {
                Identification = dto.Identification,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _db.Persons.Add(person);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIdentification), new { identification = person.Identification }, person);
        }

        // PUT: api/persons/{identification}
        [HttpPut("{identification}")]
        public async Task<IActionResult> UpdatePerson(string identification, [FromBody] UpdatePersonDto dto)
        {
            var person = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);
            if (person == null) return NotFound();

            person.FirstName = dto.FirstName ?? person.FirstName;
            person.LastName = dto.LastName ?? person.LastName;
            person.Email = dto.Email ?? person.Email;
            person.Phone = dto.Phone ?? person.Phone;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

