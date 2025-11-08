using AutoMapper;
using Firmeza.API.DTOs;
using Firmeza.API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<IdentityUser> userManager,
            IJwtService jwtService,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<(bool Success, string Token, UserDTO? User)> LoginAsync(LoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return (false, "El correo y la contraseña son obligatorios", null);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return (false, string.Empty, null);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = roles;

            return (true, token, userDto);
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                return (false, "El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return (false, "El correo electrónico es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return (false, "La contraseña es obligatoria.");

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return (false, "El número de teléfono es obligatorio.");

            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
                return (false, "El correo ya está registrado");

            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Cliente");
            return (true, "Usuario registrado correctamente");
        }
    }
}
