using PersonVehicle.BL;
using PersonVehicle.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonVehicle.DA
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Persons?> ObtenerIdentificacionAsync(int identification)
        {
            return await _context.Persons.FirstOrDefaultAsync(p => p.Identification == identification);
        }
        public async Task<Persons?> ObtenerPersonPorIdAsync(int id)
        {
            return await _context.Persons.FirstOrDefaultAsync(p => p.idPerson == id);
        }
        public async Task<IEnumerable<Persons>> ObtenerListaPersonAsync()
        {
            return await _context.Persons
                .Include(p => p.Owners)
                    .ThenInclude(o => o.Vehicle)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<msjResp>> AgregarPersonAsync(Persons persona)
        {
            await _context.Persons.AddAsync(persona); //Personas es Tabla Personas
            await _context.SaveChangesAsync();
            return await _context.msjResp.ToListAsync();
        }
        public async Task ActualizarPersonAsync(Persons persona)
        {
            _context.Persons.Update(persona);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarPersonAsync(int identification)
        {
            //var persona = await ObtenerIdentificacionAsync(identification);

            //var ppp = await _context.Persons
            //    .Include(p => p.Owners)
            //        .ThenInclude(o => o.Vehicle)
            //    .FirstOrDefaultAsync(p => p.Identification == identification);
            var persona = await _context.Persons
                .Include(p => p.Owners)
                    .ThenInclude(o => o.Vehicle)
                .FirstOrDefaultAsync(p => p.Identification == identification);
            if (persona != null)
            {
                _context.Persons.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }


        //public async Task<bool> EliminarPersonAsync(int identification)
        //{
        //    var persona = await _context.Persons
        //        .Include(p => p.Owners)
        //            .ThenInclude(o => o.Vehicle)
        //        .FirstOrDefaultAsync(p => p.Identification == identification);

        //    if (persona == null)
        //        return false;

        //    // 1. Eliminar vehículos
        //    foreach (var owner in persona.Owners)
        //    {
        //        if (owner.Vehicle != null)
        //            _context.Vehicles.Remove(owner.Vehicle);
        //    }

        //    // 2. Eliminar owners
        //    _context.Owners.RemoveRange(persona.Owners);

        //    // 3. Eliminar persona
        //    _context.Persons.Remove(persona);

        //    await _context.SaveChangesAsync();
        //    return true;
        //}




        public async Task<Persons?> ObtenerListaxIdentificationAsync(int identification)
        {
            return await _context.Persons
                .Include(p => p.Owners)
                    .ThenInclude(o => o.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identification == identification);
        }
    }
}
