using PersonVehicle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonVehicle.BL
{
    public interface IVehicleRepository
    {
        Task<Vehicles?> ObtenerVehiclePorIdAsync(int id);
        Task<Vehicles?> ObtenerVehiclePorPlateAsync(string plate);
        Task<IEnumerable<Vehicles>> ObtenerVehicleAsync();
        //Task<IEnumerable<msjResp>> AgregarVehicleAsync(Vehicle vehicle);
        Task<int> AgregarVehicleAsync(Vehicles vehicle);
        Task ActualizarVehicleAsync(Vehicles vehicle);
        Task EliminarVehicleAsync(string plate);
        Task<Vehicles?> ObtenerVehiculoConOwnerAsync(string plate);
    }
}
