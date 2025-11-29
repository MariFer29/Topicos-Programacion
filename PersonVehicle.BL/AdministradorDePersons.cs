using PersonVehicle.Model;


namespace PersonVehicle.BL
{
    public class AdministradorDePersons : IAdministradorDePersons
    {
        private readonly IPersonRepository _personaRepository;

        public AdministradorDePersons(IPersonRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public async Task<IEnumerable<msjResp>> AgreguePersonAsync(Persons person)
        {
            //PERSONA
            var personaExistente = await _personaRepository.ObtenerIdentificacionAsync(person.Identification);
            if (personaExistente != null)
            {
                var Mensaje = new msjResp { id = 0, Mensaje = $"❗ La cédula {person.Identification} ya está registrada." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            if (person.Identification.ToString().Length != 9)
            {
                var Mensaje = new msjResp { id = -1, Mensaje = "❗La cédula de la persona no puede ser 0 ni tener más de 9 digitos." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            if (String.IsNullOrEmpty(person.FirstName))
            {
                var Mensaje = new msjResp { id = -2, Mensaje = "❗El Nombre de la persona no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            if (String.IsNullOrEmpty(person.LastName))
            {
                var Mensaje = new msjResp { id = -3, Mensaje = "❗El Primer Apellido de la persona no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            //Correo
            if (String.IsNullOrEmpty(person.Email))
            {
                var Mensaje = new msjResp { id = -4, Mensaje = "❗El correo de la persona no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            //Telefono
            if (person.Phone <= 0 || person.Phone < 9)
            {
                var Mensaje = new msjResp { id = -5, Mensaje = "❗El numero de telefono de la persona no puede ser 0 ni tener más de 8 digitos." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            //Salario
            if (person.Salario <= 0 )
            {
                var Mensaje = new msjResp { id = -6, Mensaje = "❗El salario de la persona no puede ser 0." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            await _personaRepository.AgregarPersonAsync(person);
            var MSJ = new msjResp { id = 1, Mensaje = $"Persona con la identificacion {person.Identification} fue registrada con exito!." };
            var MSJS = new List<msjResp>();
            MSJS.Add(MSJ);
            return MSJS;
        }
        public async Task<String> ActualizarPersonAsync(int identification, Persons person) //BL
        {
            var PersonaAMOdificar = await _personaRepository.ObtenerIdentificacionAsync(identification);
            if (PersonaAMOdificar == null)
            {
                return $"❗La persona con el id {identification} no fue encontrado.";

            }
            PersonaAMOdificar.Identification = person.Identification;
            PersonaAMOdificar.FirstName = person.FirstName;
            PersonaAMOdificar.LastName = person.LastName;
            PersonaAMOdificar.Email = person.Email;
            PersonaAMOdificar.Phone = person.Phone;
            PersonaAMOdificar.Salario = person.Salario;
            await _personaRepository.ActualizarPersonAsync(PersonaAMOdificar);
            return $"La persona con la identification  {identification} fue actualizado con éxito.";
        }
        public async Task<String> EliminarPersonAsync(int identification) //BL
        {
            var PersonAEliminar = await _personaRepository.ObtenerIdentificacionAsync(identification);
            if (PersonAEliminar == null)
            {
                return $"La persona con el id {identification} no fue encontrado.";
            }
            await _personaRepository.EliminarPersonAsync(identification);
            return $"La persona con la identification  {identification} fue eliminado con éxito.";
        }
        public async Task<IEnumerable<Persons>> ObtengaListaPersonsAsync()
        {
            return await _personaRepository.ObtenerListaPersonAsync();
        }
        public async Task<Persons?> ObtengaLaPersonaAsync(int id)
        {
            return await _personaRepository.ObtenerPersonPorIdAsync(id);
        }
        public async Task<Persons> ObtenerListaxIdentificationAsync(int identification)
        {
            var PersonIDE = await _personaRepository.ObtenerListaxIdentificationAsync(identification);

            if (PersonIDE == null)
            {
                throw new Exception($"La persona con la identificación {identification} no fue encontrada.");
            }

            return PersonIDE;
        }
    }
}
