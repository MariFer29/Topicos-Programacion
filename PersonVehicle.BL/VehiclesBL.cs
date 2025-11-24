using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;
using PersonVehicleApi.Model;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.BL
{
    public class VehiclesBL
    {
        private readonly AppDbContext _db;

        // Constructor con inyección del DbContext
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

        // Obtener vehículos por identificación del dueño
        public async Task<(bool Success, string Message, List<Vehicle>? Vehicles)> GetByOwnerAsync(string identification)
        {
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);

            if (owner == null)
                return (false, "Owner not found", null);

            var vehicles = await _db.Vehicles
                .Include(v => v.Owner) // incluir información del dueño
                .Where(v => v.OwnerId == owner.Id)
                .AsNoTracking()
                .ToListAsync();

            return (true, "Vehicles retrieved", vehicles);
        }

        // Obtener dueño mediante la placa del vehículo
        public async Task<(bool Success, string Message, Person? Owner)> GetOwnerByPlateAsync(string plate)
        {
            // Busca el vehículo con su dueño incluido
            var vehicle = await _db.Vehicles
                .Include(v => v.Owner)
                .FirstOrDefaultAsync(v => v.Plate == plate);

            // Si no existe el vehículo, devuelve error
            if (vehicle == null)
                return (false, "Vehicle not found", null);

            // Retorna el dueño del vehículo
            return (true, "Owner retrieved", vehicle.Owner);
        }

        // Crear un vehículo nuevo
        public async Task<(bool Success, string Message, Vehicle? CreatedVehicle)> CreateVehicleAsync(CreateVehicleDto dto)
        {
            // Verificar que exista la persona dueño
            var owner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == dto.OwnerIdentification);

            if (owner == null)
                return (false, "Owner (identification) not found", null);

            // Verificar placa duplicada
            if (await _db.Vehicles.AnyAsync(v => v.Plate == dto.Plate))
                return (false, "Vehicle with this plate already exists", null);

            // Crear el vehículo
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

            // Actualizar solo los campos enviados
            vehicle.Make = dto.Make ?? vehicle.Make;
            vehicle.Model = dto.Model ?? vehicle.Model;
            vehicle.Year = dto.Year ?? vehicle.Year;

            await _db.SaveChangesAsync();

            return (true, "Vehicle updated successfully");
        }

        // Cambiar el dueño de un vehículo
        public async Task<(bool Success, string Message)> UpdateVehicleOwnerAsync(string plate, string newOwnerIdentification)
        {
            // Buscar vehículo
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);
            if (vehicle == null)
                return (false, "Vehicle not found");

            // Buscar nuevo dueño por identificación
            var newOwner = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == newOwnerIdentification);
            if (newOwner == null)
                return (false, "New owner not found");

            // Asignar nuevo dueño
            vehicle.OwnerId = newOwner.Id;

            await _db.SaveChangesAsync();

            return (true, "Vehicle owner updated successfully");
        }

        // Eliminar un vehículo por placa
        public async Task<(bool Success, string Message)> DeleteVehicleAsync(string plate)
        {
            // Buscar el vehículo en la base de datos
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null)
                return (false, "Vehicle not found");

            // Eliminar vehículo
            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync();

            return (true, "Vehicle deleted successfully");
        }
    }
}


