using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AdminManager.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace AdminManager.Web.Pages.Admin
{
    [Authorize]
    public class SalesModel : PageModel
    {
        private readonly AppDbContext _context;

        public SalesModel(AppDbContext context)
        {
            _context = context;
        }

        public List<SaleView> Sales { get; set; } = new();
        public int TotalSales { get; set; }

        public async Task OnGetAsync()
        {
            var data = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .ToListAsync();

            Sales = data.Select(s => new SaleView
            {
                Date = s.Date,
                ClientName = s.Client?.UserName ?? "Sin cliente",
                ProductCount = s.SaleProducts.Count,
                Total = s.SaleProducts.Sum(sp => sp.UnitPrice * sp.Quantity)
            }).ToList();

            TotalSales = Sales.Count;
        }

        public class SaleView
        {
            public DateTime Date { get; set; }
            public string ClientName { get; set; } = string.Empty;
            public int ProductCount { get; set; }
            public decimal Total { get; set; }
        }
    }
}

