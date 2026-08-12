using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RadioApi.Services;

namespace RadioApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RadioController(IRadioService service) : ControllerBase
{
    [HttpGet("stations/{tag}")]
    public async Task<IActionResult> GetStationsByTag(
        string tag,
        [FromQuery, Range(1, 100)] int limit = 20,
        [FromQuery, Range(0, int.MaxValue)] int offset = 0
    ) =>
        Ok(await service.GetStationsByTag(tag, limit, offset));

    [HttpGet("tags/all")]
    public async Task<IActionResult> GetAllTags(
        [FromQuery, Range(1, 500)] int limit = 20,
        [FromQuery, Range(0, int.MaxValue)] int offset = 0)
    {
        return Ok(await service.GetAllTags(limit, offset));
    }

    [HttpGet("tags/search/{tag}")]
    public async Task<IActionResult> SearchTags(
        string tag,
        [FromQuery, Range(1, 100)] int limit = 20,
        [FromQuery, Range(0, int.MaxValue)] int offset = 0
        )
    {
        var tags = await service.GetTagsSearch(tag, limit, offset);
        return Ok(tags);
    }

    [HttpGet("stations/search")]
    public async Task<IActionResult> SearchStations(
        [FromQuery] string tag = "",
        [FromQuery] string name = "",
        [FromQuery] string language = "",
        [FromQuery, Range(1, 100)] int limit = 20,
        [FromQuery, Range(0, int.MaxValue)] int offset = 0
    )
    {
        return Ok(await service.GetStationsSearch(name, language, tag, limit, offset));
    }

    [HttpGet("stations/uuid/{uuid}")]
    public async Task<IActionResult> GetStationByUuid(string uuid)
    {
        return Ok(await service.GetStationByUuid(uuid));
    }
}