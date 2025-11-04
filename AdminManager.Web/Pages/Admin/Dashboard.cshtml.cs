using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AdminManager.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace AdminManager.Web.Pages.Admin
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardModel(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int TotalProducts { get; set; }
        public int TotalSales { get; set; }
        public int TotalClients { get; set; }
        public decimal TotalRevenue { get; set; }

        public async Task OnGetAsync()
        {
            TotalProducts = await _context.Products.CountAsync();
            TotalSales = await _context.Sales.CountAsync();
            TotalRevenue = await _context.SaleProducts.SumAsync(sp => sp.UnitPrice * sp.Quantity);

            // Contar solo usuarios con rol "Client"
            var allUsers = _userManager.Users.ToList();
            int clientCount = 0;

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Client"))
                    clientCount++;
            }

            TotalClients = clientCount;
        }
    }
}



