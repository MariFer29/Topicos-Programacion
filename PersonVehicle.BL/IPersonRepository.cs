using PersonVehicle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonVehicle.BL
{
    public interface IPersonRepository
    {
        Task<Persons?> ObtenerIdentificacionAsync(int identification);
        Task<Persons?> ObtenerPersonPorIdAsync(int id);
        Task<IEnumerable<Persons>> ObtenerListaPersonAsync();
        Task<IEnumerable<msjResp>> AgregarPersonAsync(Persons persona);
        Task ActualizarPersonAsync(Persons persona);
        Task EliminarPersonAsync(int identification);
        Task<Persons?> ObtenerListaxIdentificationAsync(int identification);
    }
}
