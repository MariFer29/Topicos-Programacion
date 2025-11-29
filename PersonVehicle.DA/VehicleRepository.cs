using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;

namespace PersonVehicle.DA
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicles?> ObtenerVehiclePorIdAsync(int id)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(p => p.idVehicle == id);
        }
        public async Task<Vehicles?> ObtenerVehiclePorPlateAsync(string plate)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(p => p.Plate == plate);
        }
        public async Task<IEnumerable<Vehicles>> ObtenerVehicleAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                    .ThenInclude(o => o.Person)
                .Select(v => new Vehicles
                {
                    Plate = v.Plate,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    PersonIdentification = v.Owner.Person.Identification,
                    PersonFirstName = v.Owner.Person.FirstName,
                    PersonLastName = v.Owner.Person.LastName
                })
                .ToListAsync();
        }
        public async Task<int> AgregarVehicleAsync(Vehicles vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle.idVehicle;
        }
        public async Task ActualizarVehicleAsync(Vehicles vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarVehicleAsync(string plate)
        {
            var vehicle = await ObtenerVehiclePorPlateAsync(plate);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Vehicles?> ObtenerVehiculoConOwnerAsync(string plate)
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                    .ThenInclude(o => o.Person)
                .Where(v => v.Plate == plate)
                .Select(v => new Vehicles
                {
                    Plate = v.Plate,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    PersonIdentification = v.Owner.Person.Identification,
                    PersonFirstName = v.Owner.Person.FirstName,
                    PersonLastName = v.Owner.Person.LastName
                })
                .FirstOrDefaultAsync();
        }
    }
}
