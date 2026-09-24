namespace RadioApi.Models;

public record RadioStation(
    string StationUuid,
    string Name,
    string UrlResolved,
    string HomePage,
    string? Favicon,
    string? Tags,
    int Bitrate
);