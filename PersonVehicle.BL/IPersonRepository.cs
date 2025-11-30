using PersonVehicle.Model;

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
