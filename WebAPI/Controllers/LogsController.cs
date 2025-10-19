using Core.CrossCuttingConcerns.Logging;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    private readonly ILogService _logService;

    public LogsController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet("getall")]
    public IActionResult GetAll()
    {
        var result = _logService.GetAll();
        if (result.Success)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    // İsterseniz filtreleme için ek metodlar ekleyebilirsiniz
    [HttpGet("getbydate")]
    public IActionResult GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = _logService.GetByDateRange(startDate, endDate);
        if (result.Success)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }
    [HttpGet("getbyuser")]
    public IActionResult GetByUser([FromQuery] string userName)
    {
        var result = _logService.GetByUser(userName);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}