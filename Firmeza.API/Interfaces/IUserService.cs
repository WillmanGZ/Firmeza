using Firmeza.API.DTOs;

namespace Firmeza.API.Interfaces
{
    public interface IUserService
    {
        Task<(bool Success, string Token, UserDTO? User)> LoginAsync(LoginDTO request);
        Task<(bool Success, string Message)> RegisterAsync(RegisterDTO request);
    }
}
