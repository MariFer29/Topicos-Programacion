using PersonVehicle.Model;


namespace PersonVehicle.BL
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetOwnerIdentification();
        Task<Owner?> ObtenerOwnerPorPlateAsync(string plate);
        Task<IEnumerable<msjResp>> AgregarPropietarioAsync(Owner owner);
        Task ActualizarPropietarioAsync(Owner owner);
    }
}
