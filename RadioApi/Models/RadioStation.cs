namespace RadioApi.Models;

public record RadioStation(
    string StationUuid,
    string Name,
    string UrlResolved,
    string? Favicon,
    string? Tags,
    int Bitrate
);