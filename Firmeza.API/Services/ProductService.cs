using AutoMapper;
using Firmeza.API.Data.Entities;
using Firmeza.API.DTOs.Product;
using Firmeza.API.Interfaces;
using Firmeza.API.Responses;

namespace Firmeza.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<object>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();

            var dto = _mapper.Map<List<ProductResponseDto>>(products);

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Productos obtenidos correctamente",
                Payload = dto
            };
        }

        public async Task<ApiResponse<object>> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "Producto no encontrado",
                    Payload = null
                };

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Producto encontrado",
                Payload = _mapper.Map<ProductResponseDto>(product)
            };
        }

        public async Task<ApiResponse<object>> CreateAsync(ProductCreateDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            await _repository.AddAsync(product);

            return new ApiResponse<object>
            {
                Code = 201,
                Success = true,
                Message = "Producto creado correctamente",
                Payload = _mapper.Map<ProductResponseDto>(product)
            };
        }

        public async Task<ApiResponse<object>> UpdateAsync(Guid id, ProductUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "Producto no encontrado",
                    Payload = null
                };

            _mapper.Map(dto, existing);

            await _repository.UpdateAsync(existing);

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Producto actualizado correctamente",
                Payload = null
            };
        }

        public async Task<ApiResponse<object>> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return new ApiResponse<object>
                {
                    Code = 404,
                    Success = false,
                    Message = "Producto no encontrado",
                    Payload = null
                };

            await _repository.DeleteAsync(id);

            return new ApiResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Producto eliminado correctamente",
                Payload = null
            };
        }
    }
}
