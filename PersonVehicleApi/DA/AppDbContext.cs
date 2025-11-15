using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.Model;

namespace PersonVehicleApi.DA
{
    public class AppDbContext : DbContext
    {
        // Constructor que recibe opciones de configuración
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tablas del sistema
        public DbSet<Person> Persons { get; set; } // Tabla de personas
        public DbSet<Vehicle> Vehicles { get; set; } // Tabla de vehículos

        // Configuración del modelo y reglas de base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // La identificación de la persona debe ser única
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Identification)
                .IsUnique();

            // La placa del vehículo debe ser única
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.Plate)
                .IsUnique();

            // Relación: una persona tiene muchos vehículos
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Vehicles)
                .WithOne(v => v.Owner)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina la persona, se eliminan sus vehículos

            base.OnModelCreating(modelBuilder);
        }
    }
}

