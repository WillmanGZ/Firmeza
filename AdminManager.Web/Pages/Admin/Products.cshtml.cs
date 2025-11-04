using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AdminManager.Web.Data.Entities;
using AdminManager.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace AdminManager.Web.Pages.Admin
{
    [Authorize]
    public class ProductsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProductsModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();
        public int TotalProducts { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();
            TotalProducts = Products.Count;
        }
    }
}
