using Firmeza.API.Data;
using Firmeza.API.Data.Entities;
using Firmeza.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.API.Repositories
{
    public class SaleProductRepository : ISaleProductRepository
    {
        private readonly AppDbContext _context;

        public SaleProductRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<SaleProduct>> GetAllAsync()
        {
            return await _context.SaleProducts.ToListAsync();
        }

        public async Task<SaleProduct?> GetByIdAsync(Guid id)
        {
            return await _context.SaleProducts.FindAsync(id);
        }

        public async Task AddAsync(SaleProduct saleProduct)
        {
            _context.SaleProducts.Add(saleProduct);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SaleProduct saleProduct)
        {
            _context.SaleProducts.Update(saleProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var saleProduct = await _context.SaleProducts.FindAsync(id);
            if (saleProduct != null)
            {
                _context.SaleProducts.Remove(saleProduct);
                await _context.SaveChangesAsync();
            }
        }
    }
}
