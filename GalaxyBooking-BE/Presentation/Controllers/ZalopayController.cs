using BAL.DTOs;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Request;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ZalopayController : BaseController
    {
        private readonly IZaloPayService _service;
        public ZalopayController(IZaloPayService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDTO request)
        {
            var result = await _service.CreateZalopayPayment(request);
            return Ok(new { redirectUrl = result });
        }
    }
}
