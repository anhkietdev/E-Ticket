using BAL.Constants;
using BAL.DTOs.ZaloPay;
using BAL.Services.Implement;
using BAL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ZalopayController : BaseController
    {
        private readonly IZaloPayService _service;
        private readonly ITicketService _ticketService;
        public ZalopayController(IZaloPayService service, ITicketService ticketService)
        {
            _service = service;
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> CheckOrderStatus()
        {
            var checkStatus = await _service.CheckOrderStatus();
            if (checkStatus == Constant.ZaloPayConfig.ZaloPaymentSuccessStatus)
            {
               await _ticketService.UpdatePaymentByAppTransId();
            }
            return Ok(true);
        }
    }
}
