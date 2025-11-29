using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;


namespace PersonVehicle.DA
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Owner>> GetOwnerIdentification()
        {
            return await _context.Owner
                .Include(o => o.Person)
                .Select(o => new Owner
                {
                    idOwner = o.idOwner,
                    OwnerIdentification = o.Person.Identification
                })
                .ToListAsync();
        }
        public async Task<Owner?> ObtenerOwnerPorPlateAsync(string plate)
        {
            return await _context.Owner
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(o => o.Vehicle.Plate == plate);
        }
        public async Task<IEnumerable<msjResp>> AgregarPropietarioAsync(Owner owner)
        {
            await _context.Owner.AddAsync(owner);
            await _context.SaveChangesAsync();
            return await _context.msjResp.ToListAsync();
        }
        public async Task ActualizarPropietarioAsync(Owner owner)
        {
            _context.Owner.Update(owner);
            await _context.SaveChangesAsync();
        }
    }
}
