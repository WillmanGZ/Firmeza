using Firmeza.API.DTOs.Product;
using Firmeza.API.Responses;

namespace Firmeza.API.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<object>> GetAllAsync();
        Task<ApiResponse<object>> GetByIdAsync(Guid id);
        Task<ApiResponse<object>> CreateAsync(ProductCreateDto dto);
        Task<ApiResponse<object>> UpdateAsync(Guid id, ProductUpdateDto dto);
        Task<ApiResponse<object>> DeleteAsync(Guid id);
    }
}
