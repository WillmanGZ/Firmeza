using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminManager.Web.Pages.Admin.Clients
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IdentityUser Client { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) return NotFound();

            Client = await _userManager.FindByIdAsync(id);
            if (Client == null) return NotFound();

            return Page();
        }
    }
}
