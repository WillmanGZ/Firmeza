using AdminManager.Web.Data;
using AdminManager.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace AdminManager.Web.Pages.Admin
{
    [Authorize]
    public class SalesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SalesModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
                Id = s.Id,
                Date = s.Date,
                ClientName = s.Client?.UserName ?? "Sin cliente",
                ProductCount = s.SaleProducts.Count,
                Total = s.SaleProducts.Sum(sp => sp.UnitPrice * sp.Quantity)
            }).ToList();

            TotalSales = Sales.Count;
        }


        public async Task<IActionResult> OnGetDownloadAllAsync()
        {
            var pdfService = HttpContext.RequestServices.GetRequiredService<AllSalesPdfService>();

            var bytes = await pdfService.GenerateAllSalesPdfAsync();

            return File(bytes, "application/pdf", "todas_las_ventas.pdf");
        }


        public class SaleView
        {
            public Guid Id { get; set; }  // ← agregado
            public DateTime Date { get; set; }
            public string ClientName { get; set; } = string.Empty;
            public int ProductCount { get; set; }
            public decimal Total { get; set; }
        }
    }

}

