using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;

namespace PersonVehicle.DA
{
    public class AppDbContext : DbContext
    {
        // Constructor que recibe la configuración de la conexión a la base de datos mediante inyección de dependencias
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets que representan tablas en la base de datos
        public DbSet<Persons> Persons { get; set; }    // Tabla de personas
        public DbSet<Vehicles> Vehicles { get; set; }  // Tabla de vehículos
        public DbSet<Owner> Owner { get; set; }        // Tabla de propietarios (relación persona-vehículo)
        public DbSet<msjResp> msjResp { get; set; }    // Tabla para mensajes (poco usual pero válida si se requiere persistir mensajes)

        // Configuración de relaciones y restricciones entre entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relación: 
            // Un Owner tiene UNA Persona
            // Una Persona puede tener MUCHOS Owners
            // Owner.Person_idPerson es la clave foránea
            // OnDelete.Cascade: si se elimina la persona, también se eliminan sus Owners
            modelBuilder.Entity<Owner>()
                .HasOne(o => o.Person)
                .WithMany(p => p.Owners)
                .HasForeignKey(o => o.Person_idPerson)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relación:
            // Un Owner tiene UN vehículo
            // Un vehículo tiene UN solo Owner (relación 1 a 1)
            // Owner.Vehicle_idVehicle es la clave foránea
            modelBuilder.Entity<Owner>()
                .HasOne(o => o.Vehicle)
                .WithOne(v => v.Owner)
                .HasForeignKey<Owner>(o => o.Vehicle_idVehicle);
        }
    }
}

