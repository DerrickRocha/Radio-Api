using Microsoft.AspNetCore.Mvc;
using RadioApi.Services;

namespace RadioApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RadioController(IRadioService service) : ControllerBase
{
    public IActionResult Get() => Ok("Radio is on");

    [HttpGet("stations/{tag}")]
    public async Task<IActionResult> GetStationsByTag(string tag, [FromQuery] int limit, [FromQuery] int offset) =>
        Ok(await service.GetStationsByTag(tag, limit, offset));

    [HttpGet("tags/all")]
    public async Task<IActionResult> GetAllTags([FromQuery] int limit, [FromQuery] int offset)
    {
        return Ok(await service.GetAllTags(limit, offset));
    }

    [HttpGet("stations/search")]
    public async Task<IActionResult> GetStations(
        [FromQuery] string tag = "",
        [FromQuery] string name = "",
        [FromQuery] string country = "",
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0
    )
    {
        return Ok(await service.GetStationsSearch(name, country, tag, limit, offset));
    }
}