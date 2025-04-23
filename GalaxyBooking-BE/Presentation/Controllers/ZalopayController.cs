using BAL.DTOs.ZaloPay;
using BAL.Helpers;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
