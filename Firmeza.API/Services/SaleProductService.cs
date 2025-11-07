using Firmeza.API.Data.Entities;
using Firmeza.API.Interfaces;

namespace Firmeza.API.Services
{
    public class SaleProductService : ISaleProductService
    {
        private readonly ISaleProductRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly ISaleRepository _saleRepository;

        public SaleProductService(
            ISaleProductRepository repository,
            IProductRepository productRepository,
            ISaleRepository saleRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _saleRepository = saleRepository;
        }

        public async Task<List<SaleProduct>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SaleProduct?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AddAsync(SaleProduct saleProduct)
        {
            try
            {
                if (saleProduct == null)
                    throw new ArgumentException("El producto de la venta no puede ser nulo.");

                if (saleProduct.SaleId == Guid.Empty)
                    throw new ArgumentException("El Id de la venta es obligatorio.");

                if (saleProduct.ProductId == Guid.Empty)
                    throw new ArgumentException("El Id del producto es obligatorio.");

                if (saleProduct.Quantity <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor que cero.");

                var sale = await _saleRepository.GetByIdAsync(saleProduct.SaleId);
                if (sale == null)
                    throw new ArgumentException("La venta no existe.");

                var product = await _productRepository.GetByIdAsync(saleProduct.ProductId);
                if (product == null)
                    throw new ArgumentException("El producto no existe.");

                await _repository.AddAsync(saleProduct);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(SaleProduct saleProduct)
        {
            try
            {
                if (saleProduct == null)
                    throw new ArgumentException("El producto de la venta no puede ser nulo.");

                var existing = await _repository.GetByIdAsync(saleProduct.Id);
                if (existing == null)
                    throw new ArgumentException("El producto de la venta no existe.");

                if (saleProduct.SaleId == Guid.Empty)
                    throw new ArgumentException("El Id de la venta es obligatorio.");

                if (saleProduct.ProductId == Guid.Empty)
                    throw new ArgumentException("El Id del producto es obligatorio.");

                if (saleProduct.Quantity <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor que cero.");

                var sale = await _saleRepository.GetByIdAsync(saleProduct.SaleId);
                if (sale == null)
                    throw new ArgumentException("La venta no existe.");

                var product = await _productRepository.GetByIdAsync(saleProduct.ProductId);
                if (product == null)
                    throw new ArgumentException("El producto no existe.");

                await _repository.UpdateAsync(saleProduct);
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
                    throw new ArgumentException("El producto de la venta no existe.");

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
