using PersonVehicle.Model;
using PersonVehicle.Model.DTO;

namespace PersonVehicle.BL
{
    public class AdministradorDeVehicles : IAdministradorDeVehicles
    {
        // Repositorios necesarios para la lógica del vehículo
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IOwnerRepository _ownerRepository;

        // Inyección de dependencias
        public AdministradorDeVehicles(IVehicleRepository vehicleRepository, IPersonRepository personRepository,
                                       IOwnerRepository ownerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _personRepository = personRepository;
            _ownerRepository = ownerRepository;
        }

        // Agregar un vehículo con validaciones y registro de propietario
        public async Task<IEnumerable<msjResp>> AgregueVehicleAsync(Vehicles vehicle)
        {
            // Validar si la placa ya está registrada
            var placaExistente = await _vehicleRepository.ObtenerVehiclePorPlateAsync(vehicle.Plate);
            if (placaExistente != null)
            {
                var Mensaje = new msjResp { id = -8, Mensaje = $"❗ El vehiculo con la placa {vehicle.Plate} ya está registrado." };
                var Mensajes = new List<msjResp>() { Mensaje };
                return Mensajes;
            }

            // Validar placa no vacía
            if (String.IsNullOrEmpty(vehicle.Plate))
            {
                var Mensaje = new msjResp { id = -9, Mensaje = "❗La placa del vehiculo no puede ser blanco." };
                return new List<msjResp>() { Mensaje };
            }

            // Validar marca
            if (String.IsNullOrEmpty(vehicle.Make))
            {
                var Mensaje = new msjResp { id = -10, Mensaje = "❗La marca del vehiculo no puede ser blanco." };
                return new List<msjResp>() { Mensaje };
            }

            // Validar modelo
            if (String.IsNullOrEmpty(vehicle.Model))
            {
                var Mensaje = new msjResp { id = -11, Mensaje = "❗EL modelo del vehiculo no puede ser blanco." };
                return new List<msjResp>() { Mensaje };
            }

            // Validar año
            if (vehicle.Year <= 0)
            {
                var Mensaje = new msjResp { id = -12, Mensaje = "❗El año del vehiculo no puede ser 0." };
                return new List<msjResp>() { Mensaje };
            }

            // Verificar que exista el dueño en la BD
            var owner = await _personRepository.ObtenerIdentificacionAsync(vehicle.PersonIdentification);
            if (owner == null)
            {
                var Mensaje = new msjResp { id = -7, Mensaje = $"❗ La identificacion {vehicle.PersonIdentification} no fue encontrada." };
                return new List<msjResp>() { Mensaje };
            }

            // Guardar vehículo y obtener ID generado
            var idVehiculo = await _vehicleRepository.AgregarVehicleAsync(vehicle);

            // Crear relación Owner (dueño - vehículo)
            var propietario = new Owner
            {
                Person_idPerson = owner.idPerson,
                Vehicle_idVehicle = vehicle.idVehicle  // Este ID viene del repositorio
            };

            // Guardar propietario
            await _ownerRepository.AgregarPropietarioAsync(propietario);

            // Mensaje de éxito
            var MSJ = new msjResp { id = 1, Mensaje = $"El vehiculo con la placa {vehicle.Plate} fue registrada con exito!." };
            return new List<msjResp>() { MSJ };
        }

        // Actualizar datos de un vehículo
        public async Task<string> ActualizarVehicleAsync(string placa, VehicleUpdateDto dto)
        {
            // Buscar vehículo original
            var vehiculoAModificar = await _vehicleRepository.ObtenerVehiclePorPlateAsync(placa);
            if (vehiculoAModificar == null)
            {
                return $"❗El vehículo con la placa {placa} no fue encontrado.";
            }

            // Si cambia la placa, verificar que no exista otra igual
            if (vehiculoAModificar.Plate != dto.Plate)
            {
                var placaExistente = await _vehicleRepository.ObtenerVehiclePorPlateAsync(dto.Plate);

                if (placaExistente != null)
                {
                    return $"❗ Ya existe un vehículo con la nueva placa {dto.Plate}.";
                }
            }

            // Actualizar campos
            vehiculoAModificar.Plate = dto.Plate;
            vehiculoAModificar.Make = dto.Make;
            vehiculoAModificar.Model = dto.Model;
            vehiculoAModificar.Year = dto.Year;

            await _vehicleRepository.ActualizarVehicleAsync(vehiculoAModificar);

            return $"✔ El vehículo con la placa {placa} fue actualizado correctamente.";
        }

        // Actualizar el propietario de un vehículo
        public async Task<String> ActualizarOwnerVehicleAsync(string placa, Owner owner)
        {
            // Verificar que existe el vehículo
            var vehiculoAModificar = await _vehicleRepository.ObtenerVehiclePorPlateAsync(placa);
            if (vehiculoAModificar == null)
                return $"❗ El vehículo con la placa {placa} no fue encontrado.";

            // Obtener registro Owner actual
            var propietarioActual = await _ownerRepository.ObtenerOwnerPorPlateAsync(placa);
            if (propietarioActual == null)
                return $"❗ El vehículo con la placa {placa} no tiene propietario registrado.";

            // Validar cédula
            if (owner.OwnerIdentification.ToString().Length != 9)
                return "❗ La cédula de la persona no puede ser 0 ni tener más de 9 digitos.";

            // Buscar nueva persona
            var nuevaPersona = await _personRepository.ObtenerIdentificacionAsync(owner.OwnerIdentification);
            if (nuevaPersona == null)
                return $"❗ La persona con identificación {owner.OwnerIdentification} no fue encontrada.";

            // Actualizar el propietario (solo cambia el id de persona)
            propietarioActual.Person_idPerson = nuevaPersona.idPerson;
            await _ownerRepository.ActualizarPropietarioAsync(propietarioActual);

            return $"✔ El vehículo con placa {placa} ahora es propiedad de {nuevaPersona.FirstName} {nuevaPersona.LastName}.";
        }

        // Eliminar vehículo por placa
        public async Task<String> EliminarVehicleAsync(string plate)
        {
            var vehiculoAEliminar = await _vehicleRepository.ObtenerVehiclePorPlateAsync(plate);
            if (vehiculoAEliminar == null)
            {
                return $"\"El vehiculo con la placa {plate} no fue encontrado.";
            }

            await _vehicleRepository.EliminarVehicleAsync(plate);
            return $"\"El vehiculo con el id  {plate} fue eliminado con éxito.";
        }

        // Obtener todos los vehículos
        public async Task<IEnumerable<Vehicles>> ObtengaListaVehiclesAsync()
        {
            return await _vehicleRepository.ObtenerVehicleAsync();
        }

        // Obtener un vehículo por placa incluyendo su propietario
        public async Task<Vehicles> ObtengaListaVehiclePlateAsync(string plate)
        {
            var VehiPlate = await _vehicleRepository.ObtenerVehiculoConOwnerAsync(plate);

            if (VehiPlate == null)
            {
                throw new Exception($"El vehiculo con la placa {plate} no fue encontrada.");
            }
            
            return VehiPlate;
        }

        // Obtener un vehículo por ID primario
        public async Task<Vehicles?> ObtengaElVehicleAsync(int id)
        {
            return await _vehicleRepository.ObtenerVehiclePorIdAsync(id);
        }
    }
}
