using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminManager.Web.Pages.Admin
{
    [Authorize]
    public class ClientsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClientsModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<IdentityUser> Clients { get; set; } = new();
        public int TotalClients { get; set; }

        public async Task OnGetAsync()
        {
            // Cargar solo los usuarios con rol "Client"
            var allUsers = _userManager.Users.ToList();
            var clientUsers = new List<IdentityUser>();

            foreach (var user in allUsers)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                if (userRole.Contains("Client"))
                {
                    clientUsers.Add(user);
                }
            }

            Clients = clientUsers;
            TotalClients = Clients.Count;
        }
    }
}

