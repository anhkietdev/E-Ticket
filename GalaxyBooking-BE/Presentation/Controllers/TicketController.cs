using BAL.DTOs;
using BAL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;

namespace Presentation.Controllers
{
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;
        private readonly IZaloPayService _zaloPayService;
        public TicketController(ITicketService ticketService, IZaloPayService zaloPayService)
        {
            _ticketService = ticketService;
            _zaloPayService = zaloPayService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketRequestDTO request)
        {
            try
            {
                request.CreatedBy = this.GetAuthorizedUserId();
                var createdTicket = await _ticketService.CreateTicket(request);
                return Ok(createdTicket);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
        }

        [HttpGet]
        [Route("getallticket/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetTicket(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _ticketService.GetTicketByUserId(this.GetAuthorizedUserId(), pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("getallticketlist/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetTickets(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _ticketService.GetTickets(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTicketById([FromQuery] Guid Id)
        {
            var result = await _ticketService.DeleteTicketById(Id);
            return Ok(result);
        }
    }
}
