using BBank.Model;

namespace BBank.Repositories.Contracts
{
    public interface IClinicLocationRepository
    {
        Task<IEnumerable<ClinicLocation>> GetAllAsync();
        Task<ClinicLocation> GetByIdAsync(int id);
        Task AddAsync(ClinicLocation clinicLocation);
        Task UpdateAsync(ClinicLocation clinicLocation);
        Task DeleteAsync(int id);
    }
}
