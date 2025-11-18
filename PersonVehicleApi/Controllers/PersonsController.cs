using Microsoft.AspNetCore.Mvc;
using PersonVehicleApi.BL;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly PersonsBL _bl; // Servicio de lógica de negocio

        // Inyección del servicio BL
        public PersonsController(PersonsBL bl)
        {
            _bl = bl;
        }

        // GET: api/persons — Obtiene todas las personas
        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _bl.GetAllPersonsAsync();
            return Ok(persons);
        }

        // GET: api/persons/by-identification/{identification}
        // Consulta una persona por su identificación
        [HttpGet("by-identification/{identification}")]
        public async Task<IActionResult> GetByIdentification(string identification)
        {
            var person = await _bl.GetByIdentificationAsync(identification);

            if (person == null)
                return NotFound("Person not found");

            return Ok(person);
        }

        // POST: api/persons — Crear una nueva persona
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto dto)
        {
            var result = await _bl.CreatePersonAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(
                nameof(GetByIdentification),
                new { identification = result.CreatedPerson!.Identification },
                result.CreatedPerson
            );
        }

        // PUT: api/persons/{identification}
        // Actualiza la persona según su identificación
        [HttpPut("{identification}")]
        public async Task<IActionResult> UpdatePerson(string identification, [FromBody] UpdatePersonDto dto)
        {
            var result = await _bl.UpdatePersonAsync(identification, dto);

            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}


