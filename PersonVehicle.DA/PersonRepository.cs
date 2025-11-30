using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;

namespace PersonVehicle.DA
{
    public class PersonRepository : IPersonRepository
    {
        // Contexto de acceso a la base de datos.
        private readonly AppDbContext _context;

        // Constructor que inyecta el contexto.
        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        // Busca una persona por identificación (campo Identification).
        public async Task<Persons?> ObtenerIdentificacionAsync(int identification)
        {
            return await _context.Persons
                .FirstOrDefaultAsync(p => p.Identification == identification);
        }

        // Busca una persona por id de tabla (campo idPerson).
        public async Task<Persons?> ObtenerPersonPorIdAsync(int id)
        {
            return await _context.Persons
                .FirstOrDefaultAsync(p => p.idPerson == id);
        }

        // Obtiene una lista de todas las personas incluyendo sus owners y vehículos.
        public async Task<IEnumerable<Persons>> ObtenerListaPersonAsync()
        {
            return await _context.Persons
                .Include(p => p.Owners)          // Carga los propietarios asociados.
                    .ThenInclude(o => o.Vehicle) // Carga los vehículos de cada owner.
                .AsNoTracking()                  // No rastrea los cambios para optimizar lectura.
                .ToListAsync();
        }

        // Agrega una nueva persona a la base de datos.
        public async Task<IEnumerable<msjResp>> AgregarPersonAsync(Persons persona)
        {
            await _context.Persons.AddAsync(persona); // Inserta una nueva fila.
            await _context.SaveChangesAsync();        // Guarda los cambios.

            // Devuelve los mensajes almacenados en msjResp (si existieran).
            return await _context.msjResp.ToListAsync();
        }

        // Actualiza los datos de una persona existente.
        public async Task ActualizarPersonAsync(Persons persona)
        {
            _context.Persons.Update(persona); // Marca la entidad como modificada.
            await _context.SaveChangesAsync(); // Guarda los cambios.
        }

        // Elimina una persona según su identificación, junto con sus vehículos asociados.
        public async Task EliminarPersonAsync(int identification)
        {
            var persona = await _context.Persons
                .Include(p => p.Owners)             // Carga los owners relacionados.
                    .ThenInclude(o => o.Vehicle)    // Carga los vehículos de cada owner.
                .FirstOrDefaultAsync(p => p.Identification == identification);

            // Si la persona existe, procede a eliminar.
            if (persona != null)
            {
                // Recorre cada owner y elimina su vehículo si existe.
                foreach (var owner in persona.Owners)
                {
                    if (owner.Vehicle != null)
                    {
                        _context.Vehicles.Remove(owner.Vehicle);
                    }
                }

                // Finalmente elimina la persona.
                _context.Persons.Remove(persona);

                await _context.SaveChangesAsync();
            }
        }

        // Obtiene una persona por identificación incluyendo owners y vehículos.
        public async Task<Persons?> ObtenerListaxIdentificationAsync(int identification)
        {
            return await _context.Persons
                .Include(p => p.Owners)
                    .ThenInclude(o => o.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identification == identification);
        }
    }
}
