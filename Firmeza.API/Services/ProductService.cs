using Firmeza.API.Data.Entities;
using Firmeza.API.Interfaces;

namespace Firmeza.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AddAsync(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentException("El producto no puede ser nulo.");

                if (string.IsNullOrWhiteSpace(product.Name))
                    throw new ArgumentException("El nombre del producto es obligatorio.");

                if (string.IsNullOrWhiteSpace(product.Description))
                    throw new ArgumentException("La descripción del producto es obligatoria.");

                if (product.Price < 0)
                    throw new ArgumentException("El precio no puede ser negativo.");

                await _repository.AddAsync(product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentException("El producto no puede ser nulo.");

                var existing = await _repository.GetByIdAsync(product.Id);

                if (existing == null)
                    throw new ArgumentException("El producto no existe.");

                if (string.IsNullOrWhiteSpace(product.Name))
                    throw new ArgumentException("El nombre del producto es obligatorio.");

                if (string.IsNullOrWhiteSpace(product.Description))
                    throw new ArgumentException("La descripción del producto es obligatoria.");

                if (product.Price < 0)
                    throw new ArgumentException("El precio no puede ser negativo.");

                await _repository.UpdateAsync(product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);

                if (existing == null)
                    throw new ArgumentException("El producto no existe.");

                await _repository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
