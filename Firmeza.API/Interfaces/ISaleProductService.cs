using Firmeza.API.Data.Entities;
using Firmeza.API.Responses;
using Firmeza.API.DTOs.SaleProduct;

namespace Firmeza.API.Interfaces
{
    public interface ISaleProductService
    {
        Task<ApiResponse<List<SaleProduct>>> GetAllAsync();
        Task<ApiResponse<SaleProduct?>> GetByIdAsync(Guid id);
        Task<ApiResponse<SaleProduct?>> CreateAsync(SaleProductCreateDto request);
        Task<ApiResponse<SaleProduct?>> UpdateAsync(Guid id, SaleProductUpdateDto request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
