using Firmeza.API.DTOs;
using Firmeza.API.Responses;

namespace Firmeza.API.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> LoginAsync(LoginDTO request);
        Task<ApiResponse<object>> RegisterAsync(RegisterDTO request);
    }
}
