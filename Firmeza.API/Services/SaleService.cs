using Firmeza.API.Data.Entities;
using Firmeza.API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public SaleService(ISaleRepository repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AddAsync(Sale sale)
        {
            try
            {
                if (sale == null)
                    throw new ArgumentException("La venta no puede ser nula.");

                if (string.IsNullOrWhiteSpace(sale.ClientId))
                    throw new ArgumentException("El Id del cliente es obligatorio.");

                var client = await _userManager.FindByIdAsync(sale.ClientId);

                if (client == null)
                    throw new ArgumentException("El cliente no existe.");

                if (sale.SaleProducts == null || !sale.SaleProducts.Any())
                    throw new ArgumentException("Debe haber al menos un producto en la venta.");

                sale.Client = client;
                await _repository.AddAsync(sale);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Sale sale)
        {
            try
            {
                if (sale == null)
                    throw new ArgumentException("La venta no puede ser nula.");

                var existing = await _repository.GetByIdAsync(sale.Id);

                if (existing == null)
                    throw new ArgumentException("La venta no existe.");

                if (string.IsNullOrWhiteSpace(sale.ClientId))
                    throw new ArgumentException("El Id del cliente es obligatorio.");

                var client = await _userManager.FindByIdAsync(sale.ClientId);
                if (client == null)
                    throw new ArgumentException("El cliente no existe.");

                if (sale.SaleProducts == null || !sale.SaleProducts.Any())
                    throw new ArgumentException("Debe haber al menos un producto en la venta.");

                sale.Client = client;
                await _repository.UpdateAsync(sale);
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
                    throw new ArgumentException("La venta no existe.");

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
