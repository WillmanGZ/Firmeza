using AdminManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminManager.Web.Pages.Excel
{
    public class ImportModel : PageModel
    {
        private readonly ExcelImportService _excel;

        public ImportModel(ExcelImportService excel)
        {
            _excel = excel;
        }

        [BindProperty]
        public IFormFile ExcelFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            using var stream = ExcelFile.OpenReadStream();
            var errors = await _excel.ImportAsync(stream);

            TempData["ImportErrors"] = string.Join("\n", errors);
            return RedirectToPage("/Admin/Products/Index");
        }
    }
}
