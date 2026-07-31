using System.Text.Json.Serialization;

namespace RadioApi.Network;

public class NetworkTag
{
    [property: JsonPropertyName("name")] public string Name { get; set; }
    [property: JsonPropertyName("stationcount")] public int StationCount { get; set; }
}