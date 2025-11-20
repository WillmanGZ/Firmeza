using AutoMapper;
using Firmeza.API.DTOs;
using Firmeza.API.Interfaces;
using Firmeza.API.Responses;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IEmailService emailService,
            IJwtService jwtService,
            IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<object>> LoginAsync(LoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = "El correo y la contraseña son obligatorios.",
                    Payload = null
                };

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return new ApiResponse<object>
                {
                    Code = 401,
                    Success = false,
                    Message = "Credenciales inválidas.",
                    Payload = null
                };

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);
            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = roles;

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Inicio de sesión exitoso.",
                Payload = token
            };
        }

        public async Task<ApiResponse<object>> RegisterAsync(RegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.PhoneNumber))
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = "Todos los campos son obligatorios.",
                    Payload = null
                };

            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
                return new ApiResponse<object>
                {
                    Code = 409,
                    Success = false,
                    Message = "El correo ya está registrado.",
                    Payload = null
                };

            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Payload = null
                };

            await _userManager.AddToRoleAsync(user, "Client");

            _emailService.SendAccountCreated(user);

            return new ApiResponse<object>
            {
                Code = 201,
                Success = true,
                Message = "Usuario registrado correctamente.",
                Payload = new { user.Email, user.UserName, user.PhoneNumber }
            };
        }
    }
}
