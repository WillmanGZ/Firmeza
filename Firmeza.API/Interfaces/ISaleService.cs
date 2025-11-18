using Firmeza.API.Data.Entities;
using Firmeza.API.DTOs.SaleProduct;
using Firmeza.API.Responses;

namespace Firmeza.API.Interfaces
{
    public interface ISaleService
    {
        Task<ApiResponse<object>> GetAllAsync();
        Task<ApiResponse<object>> GetByIdAsync(Guid id);
        Task<ApiResponse<object>> CreateAsync(SaleCreateDto request);
        Task<ApiResponse<object>> UpdateAsync(Guid id, SaleUpdateDto request);
        Task<ApiResponse<object>> DeleteAsync(Guid id);
    }
}
