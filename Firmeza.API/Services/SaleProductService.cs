using AutoMapper;
using Firmeza.API.Data.Entities;
using Firmeza.API.DTOs.SaleProduct;
using Firmeza.API.Interfaces;
using Firmeza.API.Responses;

namespace Firmeza.API.Services
{
    public class SaleProductService : ISaleProductService
    {
        private readonly ISaleProductRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public SaleProductService(
            ISaleProductRepository repository,
            IProductRepository productRepository,
            ISaleRepository saleRepository,
            IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<SaleProduct>>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();

            return new ApiResponse<List<SaleProduct>>
            {
                Code = 200,
                Success = true,
                Message = "Lista obtenida correctamente",
                Payload = list
            };
        }

        public async Task<ApiResponse<SaleProduct?>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 400,
                    Success = false,
                    Message = "El Id es inválido",
                    Payload = null
                };
            }

            var sp = await _repository.GetByIdAsync(id);

            if (sp == null)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 404,
                    Success = false,
                    Message = "El SaleProduct no existe",
                    Payload = null
                };
            }

            return new ApiResponse<SaleProduct?>
            {
                Code = 200,
                Success = true,
                Message = "SaleProduct encontrado",
                Payload = sp
            };
        }

        public async Task<ApiResponse<SaleProduct?>> CreateAsync(SaleProductCreateDto request)
        {
            if (request.SaleId == Guid.Empty ||
                request.ProductId == Guid.Empty ||
                request.Quantity <= 0 ||
                request.UnitPrice <= 0)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 400,
                    Success = false,
                    Message = "Todos los campos son obligatorios y deben ser válidos.",
                    Payload = null
                };
            }

            var sale = await _saleRepository.GetByIdAsync(request.SaleId);
            if (sale == null)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 404,
                    Success = false,
                    Message = "La venta no existe",
                    Payload = null
                };
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 404,
                    Success = false,
                    Message = "El producto no existe",
                    Payload = null
                };
            }

            var saleProduct = _mapper.Map<SaleProduct>(request);
            await _repository.AddAsync(saleProduct);

            return new ApiResponse<SaleProduct?>
            {
                Code = 201,
                Success = true,
                Message = "SaleProduct creado correctamente",
                Payload = saleProduct
            };
        }

        public async Task<ApiResponse<SaleProduct?>> UpdateAsync(Guid id, SaleProductUpdateDto request)
        {
            if (id == Guid.Empty)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 400,
                    Success = false,
                    Message = "El Id es inválido",
                    Payload = null
                };
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 404,
                    Success = false,
                    Message = "El SaleProduct no existe",
                    Payload = null
                };
            }

            if (request.SaleId == Guid.Empty ||
                request.ProductId == Guid.Empty ||
                request.Quantity <= 0 ||
                request.UnitPrice <= 0)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 400,
                    Success = false,
                    Message = "Todos los campos son obligatorios y deben ser válidos.",
                    Payload = null
                };
            }

            var sale = await _saleRepository.GetByIdAsync(request.SaleId);
            if (sale == null)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 404,
                    Success = false,
                    Message = "La venta no existe",
                    Payload = null
                };
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return new ApiResponse<SaleProduct?>
                {
                    Code = 404,
                    Success = false,
                    Message = "El producto no existe",
                    Payload = null
                };
            }

            existing.SaleId = request.SaleId;
            existing.ProductId = request.ProductId;
            existing.Quantity = request.Quantity;
            existing.UnitPrice = request.UnitPrice;

            await _repository.UpdateAsync(existing);

            return new ApiResponse<SaleProduct?>
            {
                Code = 200,
                Success = true,
                Message = "SaleProduct actualizado correctamente",
                Payload = existing
            };
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new ApiResponse<string>
                {
                    Code = 400,
                    Success = false,
                    Message = "El Id es inválido",
                    Payload = null
                };
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ApiResponse<string>
                {
                    Code = 404,
                    Success = false,
                    Message = "El SaleProduct no existe",
                    Payload = null
                };
            }

            await _repository.DeleteAsync(id);

            return new ApiResponse<string>
            {
                Code = 200,
                Success = true,
                Message = "SaleProduct eliminado correctamente",
                Payload = "Deleted"
            };
        }
    }

}
