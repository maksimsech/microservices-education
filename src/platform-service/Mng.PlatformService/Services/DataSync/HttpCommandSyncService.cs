using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mng.PlatformService.DataContracts;
using Mng.PlatformService.Options;

namespace Mng.PlatformService.Services.DataSync;

public class HttpCommandSyncService : ICommandSyncService
{
    private readonly HttpClient _httpClient;
    private readonly HttpCommandSyncServiceOptions _options;

    public HttpCommandSyncService(
        HttpClient httpClient,
        IOptions<HttpCommandSyncServiceOptions> options
    )
    {
        _httpClient = httpClient;
        _options = options.Value;

        _httpClient.BaseAddress = new Uri(_options.BaseAddress);
    }

    public async Task SendPlatformAsync(PlatformReadDataContract platform)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        var responseMessage = await _httpClient.PostAsync(_options.AddPlatform, content);

        responseMessage.EnsureSuccessStatusCode();
    }
}