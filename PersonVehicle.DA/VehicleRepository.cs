using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;

namespace PersonVehicle.DA
{
    public class VehicleRepository : IVehicleRepository
    {
        // Contexto para acceder a la base de datos.
        private readonly AppDbContext _context;

        // Constructor que recibe el contexto mediante inyección de dependencias.
        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene un vehículo por su ID en la base de datos.
        public async Task<Vehicles?> ObtenerVehiclePorIdAsync(int id)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(p => p.idVehicle == id);
        }

        // Obtiene un vehículo por su número de placa.
        public async Task<Vehicles?> ObtenerVehiclePorPlateAsync(string plate)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(p => p.Plate == plate);
        }

        // Devuelve la lista de vehículos incluyendo información del propietario y persona.
        public async Task<IEnumerable<Vehicles>> ObtenerVehicleAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Owner)              // Carga la relación con el propietario.
                    .ThenInclude(o => o.Person)     // Carga la persona asociada al owner.
                .Select(v => new Vehicles           // Proyección para devolver solo campos necesarios.
                {
                    Plate = v.Plate,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    PersonIdentification = v.Owner.Person.Identification
                })
                .ToListAsync();
        }

        // Agrega un nuevo vehículo a la base de datos y retorna el ID generado.
        public async Task<int> AgregarVehicleAsync(Vehicles vehicle)
        {
            _context.Vehicles.Add(vehicle);  // Agrega el vehículo al contexto.
            await _context.SaveChangesAsync(); // Guarda los cambios.
            return vehicle.idVehicle;        // Devuelve el ID asignado.
        }

        // Actualiza los datos de un vehículo existente.
        public async Task ActualizarVehicleAsync(Vehicles vehicle)
        {
            _context.Vehicles.Update(vehicle);   // Marca como entidad modificada.
            await _context.SaveChangesAsync();   // Guarda los cambios.
        }

        // Elimina un vehículo según la placa especificada.
        public async Task EliminarVehicleAsync(string plate)
        {
            var vehicle = await ObtenerVehiclePorPlateAsync(plate); // Busca el vehículo.

            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle); // Elimina el registro.
                await _context.SaveChangesAsync();
            }
        }

        // Obtiene un vehículo junto con su owner y la identificación de la persona.
        public async Task<Vehicles?> ObtenerVehiculoConOwnerAsync(string plate)
        {
            return await _context.Vehicles
                .Include(v => v.Owner)             // Carga el owner asociado.
                    .ThenInclude(o => o.Person)    // Carga la persona del owner.
                .Where(v => v.Plate == plate)
                .Select(v => new Vehicles          // Proyección limpia.
                {
                    Plate = v.Plate,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    PersonIdentification = v.Owner.Person.Identification
                })
                .FirstOrDefaultAsync();
        }
    }
}

