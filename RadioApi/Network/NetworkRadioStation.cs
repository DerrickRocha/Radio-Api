using System.Text.Json.Serialization;

namespace RadioApi.Network;

public record NetworkRadioStation(
    [property: JsonPropertyName("stationuuid")] string StationUuid,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url_resolved")] string UrlResolved,
    [property: JsonPropertyName("favicon")] string? Favicon,
    [property: JsonPropertyName("tags")] string? Tags,
    [property: JsonPropertyName("bitrate")] int Bitrate);