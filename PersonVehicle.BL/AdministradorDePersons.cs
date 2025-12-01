using PersonVehicle.Model;
using PersonVehicle.Model.DTO;

namespace PersonVehicle.BL
{
    public class AdministradorDePersons : IAdministradorDePersons
    {
        // Repositorio de acceso a datos para la entidad Persons
        private readonly IPersonRepository _personaRepository;

        // Se inyecta el repositorio por dependencia
        public AdministradorDePersons(IPersonRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        // Agregar una nueva persona validando todos sus datos
        public async Task<IEnumerable<msjResp>> AgreguePersonAsync(Persons person)
        {
            // Validación: verificar si la identificación ya existe
            var personaExistente = await _personaRepository.ObtenerIdentificacionAsync(person.Identification);
            if (personaExistente != null)
            {
                var Mensaje = new msjResp { id = 0, Mensaje = $"❗ La cédula {person.Identification} ya está registrada." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }

            // Validación: longitud de la cédula
            if (person.Identification.ToString().Length != 9)
            {
                var Mensaje = new msjResp { id = -1, Mensaje = "❗La cédula de la persona no puede ser 0 ni tener más de 9 digitos." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }

            // Validación: nombre obligatorio
            if (String.IsNullOrEmpty(person.FirstName))
            {
                var Mensaje = new msjResp { id = -2, Mensaje = "❗El Nombre de la persona no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }

            // Validación: primer apellido obligatorio
            if (String.IsNullOrEmpty(person.LastName))
            {
                var Mensaje = new msjResp { id = -3, Mensaje = "❗El Primer Apellido de la persona no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }

            // Validación: correo obligatorio
            if (String.IsNullOrEmpty(person.Email))
            {
                var Mensaje = new msjResp { id = -4, Mensaje = "❗El correo de la persona no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }

            // Validación: teléfono correcto
            if (person.Phone.ToString().Length != 8)
            {
                var Mensaje = new msjResp { id = -5, Mensaje = "❗El número de teléfono debe tener exactamente 8 dígitos." };
                return new List<msjResp> { Mensaje };
            }

            // Validación: salario mayor a cero
            if (person.Salario <= 0)
            {
                var Mensaje = new msjResp { id = -6, Mensaje = "❗El salario de la persona no puede ser 0." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }

            // Si todas las validaciones pasan, se agrega la persona
            await _personaRepository.AgregarPersonAsync(person);

            // Mensaje de éxito
            var MSJ = new msjResp { id = 1, Mensaje = $"Persona con la identificacion {person.Identification} fue registrada con exito!." };
            var MSJS = new List<msjResp>();
            MSJS.Add(MSJ);
            return MSJS;
        }

        // Actualizar los datos de una persona existente
        public async Task<string> ActualizarPersonAsync(int identification, UpdatePersonDto dto)
        {
            // Se obtiene la persona por su identificación
            var PersonaAMOdificar = await _personaRepository.ObtenerIdentificacionAsync(identification);

            // Si no existe, se devuelve mensaje
            if (PersonaAMOdificar == null)
                return $"❗La persona con la identificación {identification} no fue encontrada.";

            // Actualización de campos
            PersonaAMOdificar.FirstName = dto.FirstName;
            PersonaAMOdificar.LastName = dto.LastName;
            PersonaAMOdificar.Email = dto.Email;
            PersonaAMOdificar.Phone = dto.Phone;
            PersonaAMOdificar.Salario = dto.Salario;

            // Se guarda la persona actualizada
            await _personaRepository.ActualizarPersonAsync(PersonaAMOdificar);

            return $"La persona con la identificación {identification} fue actualizada con éxito.";
        }

        // Eliminar una persona por identificación
        public async Task<String> EliminarPersonAsync(int identification)
        {
            // Obtener la persona a eliminar
            var PersonAEliminar = await _personaRepository.ObtenerIdentificacionAsync(identification);

            // Validar si existe
            if (PersonAEliminar == null)
            {
                return $"La persona con el id {identification} no fue encontrado.";
            }

            // Ejecución del borrado
            await _personaRepository.EliminarPersonAsync(identification);

            return $"La persona con la identification  {identification} fue eliminado con éxito.";
        }

        // Obtener la lista completa de personas
        public async Task<IEnumerable<Persons>> ObtengaListaPersonsAsync()
        {
            return await _personaRepository.ObtenerListaPersonAsync();
        }

        // Obtener una persona por ID interno
        public async Task<Persons?> ObtengaLaPersonaAsync(int id)
        {
            return await _personaRepository.ObtenerPersonPorIdAsync(id);
        }

        // Obtener una persona por su número de identificación (cédula)
        public async Task<Persons> ObtenerListaxIdentificationAsync(int identification)
        {
            var PersonIDE = await _personaRepository.ObtenerListaxIdentificationAsync(identification);

            // Si no se encuentra, lanzar excepción
            if (PersonIDE == null)
            {
                throw new Exception($"La persona con la identificación {identification} no fue encontrada.");
            }

            return PersonIDE;
        }
    }
}
