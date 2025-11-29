using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using static System.Collections.Specialized.BitVector32;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IAdministradorDePersons _adpersonRepository;

        // Inyección del servicio BL
        public PersonsController(IAdministradorDePersons adpersonRepository)
        {
            _adpersonRepository = adpersonRepository;
        }

        // GET: api/persons — Obtiene todas las personas
        [HttpGet("/api/ServicioDePersonasVehiculos/Personas/ObtengaListaDePersonas")]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _adpersonRepository.ObtengaListaPersonsAsync();
            return Ok(persons);
        }

        // GET: api/persons/by-identification/{identification}
        // Consulta una persona por su identificación
        [HttpGet("/api/ServicioDePersonasVehiculos/Personas/ObtengaListaDePersonas/PorIdentificacion/{identification}")]
        public async Task<IActionResult> GetByIdentification(int identification)
        {
            try
            {
                var result = await _adpersonRepository.ObtenerListaxIdentificationAsync(identification);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }

        // POST: api/persons — Crear una nueva persona
        [HttpPost("/api/ServicioDePersonasVehiculos/Personas/AgregueNuevaPersona")]
        public async Task<IActionResult> CreatePerson([FromBody] Persons person)
        {
            try
            {
                var result = await _adpersonRepository.AgreguePersonAsync(person);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }

        // PUT: api/persons/{identification}
        // Actualiza la persona según su identificación
        [HttpPut("/api/ServicioDePersonasVehiculos/Personas/ModifiqueLaPersona/{identification}")]
        public async Task<IActionResult> UpdatePerson(int identification, [FromBody] Persons person)
        {
            try
            {
                var result = await _adpersonRepository.ActualizarPersonAsync(identification, person);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }

        // DELETE: api/persons/{identification}
        // Elimina una persona y sus vehículos asociados
        [HttpDelete("/api/ServicioDePersonasVehiculos/Personas/ElimineLaPersona/{identification}")]
        public async Task<IActionResult> DeletePerson(int identification)
        {
            try
            {
                var result = await _adpersonRepository.EliminarPersonAsync(identification);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //message detalle error
            }
        }
    }
}