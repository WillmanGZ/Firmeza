using AdminManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminManager.Web.Pages.Excel
{
    public class DownloadTemplateModel : PageModel
    {
        private readonly ExcelImportService _excelService;

        public DownloadTemplateModel(ExcelImportService excelService)
        {
            _excelService = excelService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var bytes = await _excelService.DownloadTemplateAsync();

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReporteCompleto.xlsx"
            );
        }
    }
}
