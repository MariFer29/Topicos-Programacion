using PersonVehicle.Model;


namespace PersonVehicle.BL
{
    public interface IAdministradorDePersons
    {
        Task<IEnumerable<msjResp>> AgreguePersonAsync(Persons person);
        Task<String> ActualizarPersonAsync(int identification, Persons person);
        Task<String> EliminarPersonAsync(int identification);
        Task<IEnumerable<Persons>> ObtengaListaPersonsAsync();
        Task<Persons?> ObtengaLaPersonaAsync(int id);
        Task<Persons> ObtenerListaxIdentificationAsync(int identification);
    }
}
