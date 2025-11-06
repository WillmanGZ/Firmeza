using Firmeza.API.Data.Entities;

namespace Firmeza.API.Interfaces
{
    public interface ISaleProductRepository
    {
        Task<List<SaleProduct>> GetAllAsync();
        Task<SaleProduct?> GetByIdAsync(Guid id);
        Task AddAsync(SaleProduct saleProduct);
        Task UpdateAsync(SaleProduct saleProduct);
        Task DeleteAsync(Guid id);
    }
}
