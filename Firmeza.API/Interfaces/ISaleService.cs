using Firmeza.API.Data.Entities;

namespace Firmeza.API.Interfaces
{
    public interface ISaleService
    {
        Task<List<Sale>> GetAllAsync();
        Task<Sale?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(Sale sale);
        Task<bool> UpdateAsync(Sale sale);
        Task<bool> DeleteAsync(Guid id);
    }
}
