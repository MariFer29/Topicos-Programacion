using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.Model;

namespace PersonVehicleApi.DA
{
    public class AppDbContext : DbContext
    {
        // Constructor que recibe las opciones necesarias para conectar a la base de datos
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Representa la tabla de personas en la base de datos
        public DbSet<Person> Persons { get; set; }

        // Representa la tabla de vehículos en la base de datos
        public DbSet<Vehicle> Vehicles { get; set; }

        // Se usa para configurar reglas, restricciones y relaciones entre entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identificación de cada persona debe ser única
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Identification)
                .IsUnique();

            // Placa del vehículo también debe ser única
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.Plate)
                .IsUnique();

            // Relación: una persona puede tener muchos vehículos
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Vehicles)      // Una persona tiene muchos vehículos
                .WithOne(v => v.Owner)         // Un vehículo tiene un dueño
                .HasForeignKey(v => v.OwnerId) // Llave foránea en vehículo
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina una persona, se eliminan sus vehículos

            base.OnModelCreating(modelBuilder);
        }
    }
}

