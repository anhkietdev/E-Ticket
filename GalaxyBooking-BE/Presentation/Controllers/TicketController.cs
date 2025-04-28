using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extension;

namespace Presentation.Controllers
{
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketRequestDTO request)
        {
            try
            {
                request.CreatedBy = this.GetAuthorizedUserId();
                var createdTicket = await _ticketService.CreateTicket(this.GetAuthorizedUserId(), request);
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
        [Route("getallmyticket/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetTicket(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _ticketService.GetTicketByUserId(this.GetAuthorizedUserId(), pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("getallticketbyuserid/{userId}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetTicketByUserId(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _ticketService.GetTicketByUserId(userId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("getallticketlist/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetTickets(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _ticketService.GetTickets(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("geticketbycurrentapptransid")]
        public async Task<IActionResult> GetTicketByCurrentAppTransId()
        {
            var result = await _ticketService.GetTicketsByCurrentAppTransId();
            return Ok(result);
        }

        [HttpGet]
        [Route("{ticketId}")]
        public async Task<IActionResult> GetTicketById(Guid ticketId)
        {
            var result = await _ticketService.GetTicketById(ticketId);
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
