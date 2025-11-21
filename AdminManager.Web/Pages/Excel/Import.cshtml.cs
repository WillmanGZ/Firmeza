using AdminManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminManager.Web.Pages.Excel
{
    public class ImportModel : PageModel
    {
        private readonly ExcelImportService _excelService;

        public ImportModel(ExcelImportService excelService)
        {
            _excelService = excelService;
        }

        [BindProperty]
        public IFormFile ExcelFile { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (ExcelFile == null)
            {
                Message = "Seleccione un archivo válido.";
                return Page();
            }

            using var stream = ExcelFile.OpenReadStream();
            var ok = await _excelService.ImportAsync(stream);

            Message = ok ? "Importación completada con éxito." : "Hubo un error importando el documento.";

            return Page();
        }
    }
}
