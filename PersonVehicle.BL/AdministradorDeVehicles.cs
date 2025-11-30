using PersonVehicle.Model;
using System;


namespace PersonVehicle.BL
{
    public class AdministradorDeVehicles : IAdministradorDeVehicles
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IOwnerRepository _ownerRepository;

        public AdministradorDeVehicles(IVehicleRepository vehicleRepository, IPersonRepository personRepository,
                                       IOwnerRepository ownerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _personRepository = personRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<msjResp>> AgregueVehicleAsync(Vehicles vehicle)
        {
            //Validar si ya existe la placa
            var placaExistente = await _vehicleRepository.ObtenerVehiclePorPlateAsync(vehicle.Plate);
            if (placaExistente != null)
            {
                var Mensaje = new msjResp { id = -8, Mensaje = $"❗ El vehiculo con la placa {vehicle.Plate} ya está registrado." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            //Validar si la placa es vacio o no
            if (String.IsNullOrEmpty(vehicle.Plate))
            {
                var Mensaje = new msjResp { id = -9, Mensaje = "❗La placa del vehiculo no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            //Validar si la marca es vacio o no
            if (String.IsNullOrEmpty(vehicle.Make))
            {
                var Mensaje = new msjResp { id = -10, Mensaje = "❗La marca del vehiculo no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            //Validar si la modelo es vacio o no
            if (String.IsNullOrEmpty(vehicle.Model))
            {
                var Mensaje = new msjResp { id = -11, Mensaje = "❗EL modelo del vehiculo no puede ser blanco." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            if (vehicle.Year <= 0)
            {
                var Mensaje = new msjResp { id = -12, Mensaje = "❗El año del vehiculo no puede ser 0." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            // Verificar que exista la persona
            var owner = await  _personRepository.ObtenerIdentificacionAsync(vehicle.PersonIdentification);
            if (owner == null)
            {
                var Mensaje = new msjResp { id = -7, Mensaje = $"❗ La identificacion {vehicle.PersonIdentification} no fue encontrada." };
                var Mensajes = new List<msjResp>();
                Mensajes.Add(Mensaje);
                return Mensajes;
            }
            //Se crea y se guarda en BD el vehiculo
            var idVehiculo = await _vehicleRepository.AgregarVehicleAsync(vehicle);

            var propietario = new Owner
            {
                Person_idPerson = owner.idPerson,
                Vehicle_idVehicle = vehicle.idVehicle

            };
            await _ownerRepository.AgregarPropietarioAsync(propietario);
            var MSJ = new msjResp { id = 1, Mensaje = $"El vehiculo con la placa {vehicle.Plate} fue registrada con exito!." };
            var MSJS = new List<msjResp>();
            MSJS.Add(MSJ);
            return MSJS;


        }
        public async Task<String> ActualizarVehicleAsync(string placa, Vehicles vehicle)
        {
            // Obtener el vehículo original por placa
            var vehiculoAModificar = await _vehicleRepository.ObtenerVehiclePorPlateAsync(placa);
            if (vehiculoAModificar == null)
            {
                return $"❗El vehículo con la placa {placa} no fue encontrado.";
            }

            //// Si la placa nueva es diferente a la original
            //if (vehiculoAModificar.Plate != vehicle.Plate)
            //{
            //    // Verificar que no exista otro vehículo con esa nueva placa
            //    var placaExistente = await _vehicleRepository.ObtenerVehiclePorPlateAsync(vehicle.Plate);

            //    if (placaExistente != null)
            //    {
            //        return $"❗ Ya existe un vehículo con la nueva placa {vehicle.Plate}.";
            //    }
            //}
            // Actualizar
            vehiculoAModificar.Plate = vehicle.Plate;
            vehiculoAModificar.Make = vehicle.Make;
            vehiculoAModificar.Model = vehicle.Model;
            vehiculoAModificar.Year = vehicle.Year;

            await _vehicleRepository.ActualizarVehicleAsync(vehiculoAModificar);

            return $"✔ El vehículo con la placa {placa} fue actualizado ahora como {vehicle.Plate}.";
        }
        public async Task<String> ActualizarOwnerVehicleAsync(string placa, Owner owner)
        {
            // Verificar que el vehículo existe
            var vehiculoAModificar = await _vehicleRepository.ObtenerVehiclePorPlateAsync(placa);
            if (vehiculoAModificar == null)
                return $"❗ El vehículo con la placa {placa} no fue encontrado.";

            // Obtener el registro Owner actual
            var propietarioActual = await _ownerRepository.ObtenerOwnerPorPlateAsync(placa);
            if (propietarioActual == null)
                return $"❗ El vehículo con la placa {placa} no tiene propietario registrado.";

            // Validar nueva identificación
            if (owner.OwnerIdentification.ToString().Length != 9)
                return "❗ La cédula de la persona no puede ser 0 ni tener más de 9 digitos.";

            // Buscar a la nueva persona mediante identificación
            var nuevaPersona = await _personRepository.ObtenerIdentificacionAsync(owner.OwnerIdentification);
            if (nuevaPersona == null)
                return $"❗ La persona con identificación {owner.OwnerIdentification} no fue encontrada.";

            // ACTUALIZAR EL OWNER (no la persona!)
            propietarioActual.Person_idPerson = nuevaPersona.idPerson;
            await _ownerRepository.ActualizarPropietarioAsync(propietarioActual);

            return $"✔ El vehículo con placa {placa} ahora es propiedad de {nuevaPersona.FirstName} {nuevaPersona.LastName}.";
        }
        public async Task<String> EliminarVehicleAsync(string plate) //BL
        {
            var PersonAEliminar = await _vehicleRepository.ObtenerVehiclePorPlateAsync(plate);
            if (PersonAEliminar == null)
            {
                return $"\"El vehiculo con la placa {plate} no fue encontrado.";
            }
            await _vehicleRepository.EliminarVehicleAsync(plate);
            return $"\"El vehiculo con el id  {plate} fue eliminado con éxito.";
        }
        public async Task<IEnumerable<Vehicles>> ObtengaListaVehiclesAsync()
        {
            return await _vehicleRepository.ObtenerVehicleAsync();
        }
        public async Task<Vehicles> ObtengaListaVehiclePlateAsync(string plate)
        {
            var VehiPlate = await _vehicleRepository.ObtenerVehiculoConOwnerAsync(plate);

            if (VehiPlate == null)
            {
                throw new Exception($"El vehiculo con la placa {plate} no fue encontrada.");
            }
            return VehiPlate;
        }
        public async Task<Vehicles?> ObtengaElVehicleAsync(int id)
        {
            return await _vehicleRepository.ObtenerVehiclePorIdAsync(id);
        }
    }
}
