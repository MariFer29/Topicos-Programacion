using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;
using PersonVehicleApi.Model;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.BL
{
    public class PersonsBL
    {
        private readonly AppDbContext _db;

        // Constructor que recibe el DbContext mediante inyección de dependencias
        public PersonsBL(AppDbContext db)
        {
            _db = db;
        }

        // Método para obtener todas las personas
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            return await _db.Persons
                .Include(p => p.Vehicles)  
                .AsNoTracking()
                .ToListAsync();
        }

        // Método para obtener una persona por identificación
        public async Task<Person?> GetByIdentificationAsync(string identification)
        {
            return await _db.Persons
                .Include(p => p.Vehicles) // incluir vehículos asociados
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identification == identification);
        }

        // Método para crear una nueva persona
        public async Task<(bool Success, string Message, Person? CreatedPerson)> CreatePersonAsync(CreatePersonDto dto)
        {
            // Validar identificación
            if (string.IsNullOrWhiteSpace(dto.Identification))
                return (false, "Identification is required.", null);

            // Verificar que no exista una persona con la misma identificación
            if (await _db.Persons.AnyAsync(p => p.Identification == dto.Identification))
                return (false, "Person with this identification already exists.", null);

            // Crear objeto persona
            var person = new Person
            {
                Identification = dto.Identification,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _db.Persons.Add(person);
            await _db.SaveChangesAsync();

            return (true, "Person created successfully", person);
        }

        // Método para actualizar una persona existente
        public async Task<(bool Success, string Message)> UpdatePersonAsync(string identification, UpdatePersonDto dto)
        {
            var person = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);

            if (person == null)
                return (false, "Person not found");

            // Actualizar campos solo si vienen con valores
            person.FirstName = dto.FirstName ?? person.FirstName;
            person.LastName = dto.LastName ?? person.LastName;
            person.Email = dto.Email ?? person.Email;
            person.Phone = dto.Phone ?? person.Phone;

            await _db.SaveChangesAsync();
            return (true, "Person updated successfully");
        }

        // Eliminar una persona por identificación
        public async Task<(bool Success, string Message)> DeletePersonAsync(string identification)
        {
            // Buscar persona en la base de datos
            var person = await _db.Persons.FirstOrDefaultAsync(p => p.Identification == identification);

            if (person == null)
                return (false, "Person not found");

            // EF Core eliminará automáticamente los vehículos por la regla Cascade
            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();

            return (true, "Person deleted successfully");
        }
    }
}

