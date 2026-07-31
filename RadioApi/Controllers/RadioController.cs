using Microsoft.AspNetCore.Mvc;
using RadioApi.Services;

namespace RadioApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RadioController(IRadioService service): ControllerBase
{
    public IActionResult Get() => Ok("Radio is on");
    
    [HttpGet("genre/{genre}")]
    public async Task<IActionResult> GetByGenre(string genre, [FromQuery] int limit, [FromQuery] int offset) => Ok(await service.GetByGenre(genre, limit, offset));

    [HttpGet("tags/all")]
    public async Task<IActionResult> GetAllTags([FromQuery] int limit, [FromQuery] int offset)
    {
        return Ok(await service.GetAllTags(limit, offset));
    }
}