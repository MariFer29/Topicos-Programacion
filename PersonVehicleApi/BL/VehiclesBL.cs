using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;
using PersonVehicleApi.Model;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.BL
{
    public class VehiclesBL
    {
        private readonly AppDbContext _db;

        // Constructor con inyección de DbContext
        public VehiclesBL(AppDbContext db)
        {
            _db = db;
        }

        // Obtener todos los vehículos
        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _db.Vehicles
                .Include(v => v.Owner) // incluir información del dueño
                .AsNoTracking()
                .ToListAsync();
        }

        // Obtener vehículos por identificación de persona
        public async Task<(bool Success, string Message, List<Vehicle>? Vehicles)> GetByOwnerAsync(string identification)
        {
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);

            if (owner == null)
                return (false, "Owner not found", null);

            var vehicles = await _db.Vehicles
                .Where(v => v.OwnerId == owner.Id)
                .AsNoTracking()
                .ToListAsync();

            return (true, "Vehicles retrieved", vehicles);
        }

        // Crear un vehículo nuevo
        public async Task<(bool Success, string Message, Vehicle? CreatedVehicle)> CreateVehicleAsync(CreateVehicleDto dto)
        {
            // Verificar que exista la persona dueño del vehículo
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == dto.OwnerIdentification);

            if (owner == null)
                return (false, "Owner (identification) not found", null);

            // Verificar placa duplicada
            if (await _db.Vehicles.AnyAsync(v => v.Plate == dto.Plate))
                return (false, "Vehicle with this plate already exists", null);

            // Crear objeto vehículo
            var vehicle = new Vehicle
            {
                Plate = dto.Plate,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                OwnerId = owner.Id
            };

            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();

            return (true, "Vehicle created successfully", vehicle);
        }

        // Actualizar un vehículo existente
        public async Task<(bool Success, string Message)> UpdateVehicleAsync(string plate, UpdateVehicleDto dto)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null)
                return (false, "Vehicle not found");

            // Actualizar solo campos enviados
            vehicle.Make = dto.Make ?? vehicle.Make;
            vehicle.Model = dto.Model ?? vehicle.Model;
            vehicle.Year = dto.Year ?? vehicle.Year;

            await _db.SaveChangesAsync();

            return (true, "Vehicle updated successfully");
        }
    }
}

