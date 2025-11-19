using Firmeza.API.DTOs.Sale;
using Firmeza.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.API.Controllers
{
    [Route("api/sales")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _saleService.GetAllAsync();
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _saleService.GetByIdAsync(id);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleCreateDto dto)
        {
            var response = await _saleService.CreateAsync(dto);
            return StatusCode(response.Code, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SaleUpdateDto dto)
        {
            var response = await _saleService.UpdateAsync(id, dto);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _saleService.DeleteAsync(id);
            return StatusCode(response.Code, response);
        }
    }
}

