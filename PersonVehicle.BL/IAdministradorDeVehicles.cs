using PersonVehicle.Model;
using PersonVehicle.Model.DTO;

namespace PersonVehicle.BL
{
    public interface IAdministradorDeVehicles
    {
        Task<IEnumerable<msjResp>> AgregueVehicleAsync(Vehicles vehicle);
        Task<string> ActualizarVehicleAsync(string placa, VehicleUpdateDto vehicleDto);
        Task<String> ActualizarOwnerVehicleAsync(string placa, Owner owner);
        Task<String> EliminarVehicleAsync(string plate);
        Task<IEnumerable<Vehicles>> ObtengaListaVehiclesAsync();
        Task<Vehicles> ObtengaListaVehiclePlateAsync(string plate);
        Task<Vehicles?> ObtengaElVehicleAsync(int id);
    }
}
