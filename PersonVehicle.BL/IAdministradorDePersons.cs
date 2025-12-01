using PersonVehicle.Model;
using PersonVehicle.Model.DTO;

namespace PersonVehicle.BL
{
    public interface IAdministradorDePersons
    {
        Task<IEnumerable<msjResp>> AgreguePersonAsync(Persons person);
        Task<String> ActualizarPersonAsync(int identification, UpdatePersonDto dto);
        Task<String> EliminarPersonAsync(int identification);
        Task<IEnumerable<Persons>> ObtengaListaPersonsAsync();
        Task<Persons?> ObtengaLaPersonaAsync(int id);
        Task<Persons> ObtenerListaxIdentificationAsync(int identification);
    }
}
