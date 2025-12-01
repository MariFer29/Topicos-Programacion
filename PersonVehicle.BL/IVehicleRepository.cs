using PersonVehicle.Model;

namespace PersonVehicle.BL
{
    public interface IVehicleRepository
    {
        Task<Vehicles?> ObtenerVehiclePorIdAsync(int id);
        Task<Vehicles?> ObtenerVehiclePorPlateAsync(string plate);
        Task<IEnumerable<Vehicles>> ObtenerVehicleAsync();
        Task<int> AgregarVehicleAsync(Vehicles vehicle);
        Task ActualizarVehicleAsync(Vehicles vehicle);
        Task EliminarVehicleAsync(string plate);
        Task<Vehicles?> ObtenerVehiculoConOwnerAsync(string plate);
    }
}
