using BBank.Data;
using BBank.Model;
using BBank.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BBank.Repositories
{
    public class ClinicLocationRepository : IClinicLocationRepository
    {
        private readonly ApplicationDbContext _context;

        public ClinicLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClinicLocation>> GetAllAsync()
        {
            return await _context.ClinicLocations.ToListAsync();
        }

        public async Task<ClinicLocation> GetByIdAsync(int id)
        {
            return await _context.ClinicLocations.FindAsync(id);
        }

        public async Task AddAsync(ClinicLocation clinicLocation)
        {
            _context.ClinicLocations.Add(clinicLocation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClinicLocation clinicLocation)
        {
            _context.Entry(clinicLocation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var clinicLocation = await _context.ClinicLocations.FindAsync(id);
            if (clinicLocation != null)
            {
                _context.ClinicLocations.Remove(clinicLocation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
