using BAL.Constants;
using BAL.Services.Interface;
using DAL.Common;
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
            bool result = false;
            if (checkStatus == Constant.ZaloPayConfig.ZaloPaymentSuccessStatus)
            {
                result = (await _ticketService.UpdatePaymentByAppTransId()).Any();
            }
            else if (checkStatus == Constant.ZaloPayConfig.ZaloPaymentErrorStatus)
            {
                result = await _ticketService.DeleteTicketByAppTransId(Guid.Parse(GlobalCache.AppTransIdCache));
            }
            return Ok(result);
        }
    }
}
