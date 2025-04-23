using BAL.DTOs.ZaloPay;
using BAL.Services.Interface;
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
        public async Task<IActionResult> Callback([FromBody] CallBackPaymentDTO request)
        {
            CallBackResponseDTO callBackResponseDTO = new CallBackResponseDTO();

            (callBackResponseDTO.ReturnCode, callBackResponseDTO.ReturnMessage) = await _service.CallBackPayment(request);

            return Ok(callBackResponseDTO);
        }
    }
}
