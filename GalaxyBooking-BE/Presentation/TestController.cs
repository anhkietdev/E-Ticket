using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public TestController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                bool canConnect = await _dbContext.Database.CanConnectAsync();

                var testQuery = await _dbContext.Database
                    .ExecuteSqlRawAsync("SELECT * FROM Users");

                return Ok(new
                {
                    ConnectionSuccessful = canConnect,
                    QueryExecuted = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
    }
}
