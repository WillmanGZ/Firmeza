using AutoMapper;
using Firmeza.API.Data.Entities;
using Firmeza.API.DTOs.SaleProduct;
using Firmeza.API.Interfaces;
using Firmeza.API.Responses;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public SaleService(
            ISaleRepository repository,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<object>> GetAllAsync()
        {
            var sales = await _repository.GetAllAsync();
            var mapped = _mapper.Map<List<SaleResponseDto>>(sales);

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Ventas obtenidas correctamente",
                Payload = mapped
            };
        }

        public async Task<ApiResponse<object>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = "Id inválido",
                    Payload = null
                };
            }

            var sale = await _repository.GetByIdAsync(id);

            if (sale == null)
            {
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "La venta no existe",
                    Payload = null
                };
            }

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Venta obtenida correctamente",
                Payload = _mapper.Map<SaleResponseDto>(sale)
            };
        }

        public async Task<ApiResponse<object>> CreateAsync(SaleCreateDto request)
        {
            // Validación básica estilo Auth/Product (sin validar SaleProducts aquí)
            if (request == null || string.IsNullOrWhiteSpace(request.ClientId))
            {
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = "El Id del cliente es obligatorio.",
                    Payload = null
                };
            }

            var client = await _userManager.FindByIdAsync(request.ClientId);
            if (client == null)
            {
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "El cliente no existe.",
                    Payload = null
                };
            }

            var sale = _mapper.Map<Sale>(request);
            sale.Client = client;

            await _repository.AddAsync(sale);

            return new ApiResponse<object>
            {
                Code = 201,
                Success = true,
                Message = "Venta creada correctamente",
                Payload = _mapper.Map<SaleResponseDto>(sale)
            };
        }

        public async Task<ApiResponse<object>> UpdateAsync(Guid id, SaleUpdateDto request)
        {
            if (id == Guid.Empty)
            {
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = "Id inválido",
                    Payload = null
                };
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "La venta no existe",
                    Payload = null
                };
            }

            if (request == null || string.IsNullOrWhiteSpace(request.ClientId))
            {
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = "El Id del cliente es obligatorio.",
                    Payload = null
                };
            }

            var client = await _userManager.FindByIdAsync(request.ClientId);
            if (client == null)
            {
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "El cliente no existe.",
                    Payload = null
                };
            }

            // Mapear cambios relevantes a la entidad existente (sin tocar SaleProducts)
            existing.Date = DateTime.Now; // opcional: actualizar fecha o mantener la original según negocio
            existing.ClientId = request.ClientId;
            existing.Client = client;
            // NOTA: no reasignamos existing.SaleProducts aquí (se gestionan por SaleProductController)

            await _repository.UpdateAsync(existing);

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Venta actualizada correctamente",
                Payload = _mapper.Map<SaleResponseDto>(existing)
            };
        }

        public async Task<ApiResponse<object>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new ApiResponse<object>
                {
                    Code = 400,
                    Success = false,
                    Message = "Id inválido",
                    Payload = null
                };
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "La venta no existe",
                    Payload = null
                };
            }

            await _repository.DeleteAsync(id);

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Venta eliminada correctamente",
                Payload = null
            };
        }
    }
}
