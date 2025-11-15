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
        private readonly AppDbContext _db; // Contexto de base de datos

        public PersonsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/persons — Obtiene todas las personas
        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _db.Persons.AsNoTracking().ToListAsync();
            return Ok(persons);
        }

        // GET: api/persons/by-identification/{identification}
        // Consulta una persona por su identificación
        [HttpGet("by-identification/{identification}")]
        public async Task<IActionResult> GetByIdentification(string identification)
        {
            var person = await _db.Persons
                .Include(p => p.Vehicles) // Incluye los vehículos de la persona
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identification == identification);

            if (person == null) return NotFound();
            return Ok(person);
        }

        // POST: api/persons — Crear una nueva persona
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto dto)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(dto?.Identification))
                return BadRequest("Identification is required.");

            // Verifica que no exista otra persona con la misma identificación
            if (await _db.Persons.AnyAsync(p => p.Identification == dto.Identification))
                return Conflict("Person with this identification already exists.");

            // Crear nuevo objeto persona
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
        // Actualiza la persona según su identificación
        [HttpPut("{identification}")]
        public async Task<IActionResult> UpdatePerson(string identification, [FromBody] UpdatePersonDto dto)
        {
            var person = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);
            if (person == null) return NotFound();

            // Actualiza sólo los campos enviados
            person.FirstName = dto.FirstName ?? person.FirstName;
            person.LastName = dto.LastName ?? person.LastName;
            person.Email = dto.Email ?? person.Email;
            person.Phone = dto.Phone ?? person.Phone;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}


