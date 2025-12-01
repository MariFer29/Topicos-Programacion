using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;

namespace PersonVehicle.DA
{
    public class OwnerRepository : IOwnerRepository
    {
        // Contexto de base de datos inyectado.
        private readonly AppDbContext _context;

        // Constructor que recibe el contexto y lo asigna al repositorio.
        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene una lista de propietarios mostrando únicamente el idOwner
        // y la identificación de la persona asociada.
        public async Task<IEnumerable<Owner>> GetOwnerIdentification()
        {
            return await _context.Owner
                .Include(o => o.Person)   // Carga la entidad relacionada Person.
                .Select(o => new Owner    // Proyección para devolver solo los campos requeridos.
                {
                    idOwner = o.idOwner,
                    OwnerIdentification = o.Person.Identification
                })
                .ToListAsync();           // Ejecuta la consulta asincrónicamente.
        }

        // Busca un propietario a partir de la placa de su vehículo.
        public async Task<Owner?> ObtenerOwnerPorPlateAsync(string plate)
        {
            return await _context.Owner
                .Include(o => o.Vehicle)                 // Incluye la relación Vehicle.
                .FirstOrDefaultAsync(o => o.Vehicle.Plate == plate);  // Busca coincidencia por placa.
        }

        // Agrega un nuevo propietario en la base de datos y devuelve los mensajes de respuesta.
        public async Task<IEnumerable<msjResp>> AgregarPropietarioAsync(Owner owner)
        {
            await _context.Owner.AddAsync(owner);        // Agrega el propietario al contexto.
            await _context.SaveChangesAsync();           // Guarda los cambios en la base.
            return await _context.msjResp.ToListAsync(); // Devuelve los mensajes generados.
        }

        // Actualiza la información de un propietario existente.
        public async Task ActualizarPropietarioAsync(Owner owner)
        {
            _context.Owner.Update(owner);   // Marca los datos como modificados.
            await _context.SaveChangesAsync(); // Guarda los cambios.
        }
    }
}
