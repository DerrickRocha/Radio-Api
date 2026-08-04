using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RadioApi.Models;
using RadioApi.Network;

namespace RadioApi.Services;

public interface IRadioService
{
    public Task<List<RadioStation>> GetStationsByTag(string tag, int limit, int offset);
    public Task<List<Tag>> GetAllTags(int limit, int offset);
}

public class RadioService(HttpClient httpClient, IMemoryCache cache): IRadioService
{
    private const string MirrorCacheKey = "RadioBrowserActiveMirror";
    
    private async Task<string> ResolveBaseUrlAsync()
    {
        if (cache.TryGetValue(MirrorCacheKey, out string? cachedUrl) && cachedUrl != null)
        {
            return cachedUrl;
        }

        try
        {
            var addresses = await Dns.GetHostAddressesAsync("all.api.radio-browser.info");
            if (addresses.Length > 0)
            {
                // Get the canonical host name behind that IP to keep SSL happy
                var hostEntry = await Dns.GetHostEntryAsync(addresses[0]);
                var targetUrl = $"https://{hostEntry.HostName}/"; 
    
                cache.Set(MirrorCacheKey, targetUrl, TimeSpan.FromMinutes(30));
                return targetUrl;
            }

        }
        catch
        {
            // Fallback mirror if DNS mapping fails
        }

        return "https://radio-browser.info";
    }

    
    public async Task<List<RadioStation>> GetStationsByTag(string tag, int limit = 20, int offset = 0)
    {
        var baseUrl = await ResolveBaseUrlAsync();
        var requestUrl = $"{baseUrl}json/stations/bytag/{Uri.EscapeDataString(tag)}?limit={limit}&offset={offset}&order=clickcount&reverse=true";

        // Optional custom backend mapping logic can go here (e.g., stripping invalid http images)
        var response = await httpClient.GetFromJsonAsync<List<NetworkRadioStation>>(requestUrl) ?? throw new Exception("Failed to fetch stations");
        return ToRadioStations(response) ?? [];
    }

    public async Task<List<Tag>> GetAllTags(int limit, int offset)
    {
        var baseUrl = await ResolveBaseUrlAsync();
        var requestUrl = $"{baseUrl}json/tags?limit={limit}&offset={offset}";
        var response = await httpClient.GetFromJsonAsync<List<NetworkTag>>(requestUrl) ?? throw new Exception("Failed to fetch tags");
        return ToTags(response)?? [];
    }

    private List<Tag> ToTags(List<NetworkTag> response)
    {
        var tags = response.Select(networkTag => 
                new Tag(networkTag.Name, networkTag.StationCount)
            );
        return [.. tags];
    }

    private List<RadioStation> ToRadioStations(List<NetworkRadioStation> stations)
    {
        return
        [
            .. stations.Select(station => new RadioStation(station.StationUuid, station.Name, station.UrlResolved,
                station.Favicon, station.Tags, station.Bitrate))
        ];
    }
}