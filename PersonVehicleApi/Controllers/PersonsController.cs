using Microsoft.AspNetCore.Mvc;
using PersonVehicle.BL;
using PersonVehicle.Model;
using PersonVehicle.Model.DTO;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IAdministradorDePersons _adpersonRepository;

        public PersonsController(IAdministradorDePersons adpersonRepository)
        {
            _adpersonRepository = adpersonRepository;
        }

        // GET: api/persons
        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var persons = await _adpersonRepository.ObtengaListaPersonsAsync();
            return Ok(persons);
        }

        // GET: api/persons/{identification}
        [HttpGet("{identification}")]
        public async Task<IActionResult> GetByIdentification(int identification)
        {
            try
            {
                var result = await _adpersonRepository.ObtenerListaxIdentificationAsync(identification);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/persons
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] Persons person)
        {
            try
            {
                var result = await _adpersonRepository.AgreguePersonAsync(person);
                var resultList = result.ToList();
                
                // Verificar si hay errores en la respuesta
                if (resultList.Any() && resultList[0].id <= 0)
                {
                    // Si el id es 0 o negativo, significa que hubo un error
                    return BadRequest(resultList[0].Mensaje);
                }
                
                // Si el id es positivo, la persona fue creada exitosamente
                return Ok(resultList[0].Mensaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/persons/{identification}
        [HttpPut("{identification}")]
        public async Task<IActionResult> UpdatePerson(int identification, [FromBody] UpdatePersonDto personDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _adpersonRepository.ActualizarPersonAsync(identification, personDto);

                if (result == null)
                    return NotFound($"No existe persona con identificación {identification}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/persons/{identification}
        [HttpDelete("{identification}")]
        public async Task<IActionResult> DeletePerson(int identification)
        {
            try
            {
                var result = await _adpersonRepository.EliminarPersonAsync(identification);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

