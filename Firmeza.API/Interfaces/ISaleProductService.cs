using Firmeza.API.Data.Entities;

namespace Firmeza.API.Interfaces
{
    public interface ISaleProductService
    {
        Task<List<SaleProduct>> GetAllAsync();
        Task<SaleProduct?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(SaleProduct saleProduct);
        Task<bool> UpdateAsync(SaleProduct saleProduct);
        Task<bool> DeleteAsync(Guid id);
    }
}
