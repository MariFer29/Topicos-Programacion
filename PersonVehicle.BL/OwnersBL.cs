using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;
using PersonVehicleApi.Model;

namespace PersonVehicleApi.BL
{
    public class OwnersBL
    {
        private readonly AppDbContext _db;

        public OwnersBL(AppDbContext db)
        {
            _db = db;
        }

        // Listar todos los vehículos con sus dueños
        public async Task<List<Vehicle>> GetAllVehiclesWithOwnerAsync()
        {
            return await _db.Vehicles
                .Include(v => v.Owner)
                .AsNoTracking()
                .ToListAsync();
        }

        // Obtener un vehículo específico con su dueño
        public async Task<Vehicle?> GetVehicleWithOwnerAsync(string plate)
        {
            return await _db.Vehicles
                .Include(v => v.Owner)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Plate == plate);
        }

        // Cambiar dueño del vehículo
        public async Task<(bool Success, string Message)> UpdateVehicleOwnerAsync(string plate, string newOwnerIdentification)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null)
                return (false, "Vehicle not found");

            var newOwner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == newOwnerIdentification);

            if (newOwner == null)
                return (false, "New owner not found");

            if (vehicle.OwnerId == newOwner.Id)
                return (false, "Vehicle already belongs to this owner");

            vehicle.OwnerId = newOwner.Id;

            await _db.SaveChangesAsync();
            return (true, "Owner updated successfully");
        }
    }
}


