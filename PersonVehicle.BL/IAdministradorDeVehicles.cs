using PersonVehicle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonVehicle.BL
{
    public interface IAdministradorDeVehicles
    {
        Task<IEnumerable<msjResp>> AgregueVehicleAsync(Vehicles vehicle);
        Task<String> ActualizarVehicleAsync(string placa, Vehicles vehicle);
        Task<String> ActualizarOwnerVehicleAsync(string placa, Owner owner);
        Task<String> EliminarVehicleAsync(string plate);
        Task<IEnumerable<Vehicles>> ObtengaListaVehiclesAsync();
        Task<Vehicles> ObtengaListaVehiclePlateAsync(string plate);
        Task<Vehicles?> ObtengaElVehicleAsync(int id);
    }
}
