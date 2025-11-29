using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;


namespace PersonVehicle.DA
{
    public class AppDbContext : DbContext
    {
        // Constructor que recibe las opciones necesarias para conectar a la base de datos
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<Persons> Persons { get; set; }
        public DbSet<Vehicles> Vehicles { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<msjResp> msjResp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>()
                .HasOne(o => o.Person)
                .WithMany(p => p.Owners)
                .HasForeignKey(o => o.Person_idPerson)
                .OnDelete(DeleteBehavior.Cascade); ;

            modelBuilder.Entity<Owner>()
                .HasOne(o => o.Vehicle)
                .WithOne(v => v.Owner)
                .HasForeignKey<Owner>(o => o.Vehicle_idVehicle)
                .OnDelete(DeleteBehavior.Cascade); ;
        }




    }
}
