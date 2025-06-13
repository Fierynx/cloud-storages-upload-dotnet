using Microsoft.Extensions.Options;
using WebDav;
using System.Net;
using NextCloudUpload;

public interface INextCloudService
{
    Task<bool> UploadAsync(Stream fileStream, string filename, string? folder);
}

public class NextCloudService : INextCloudService
{
    private readonly IWebDavClient _client;
    private readonly string _baseUrl;

    public NextCloudService(IOptions<NextCloudOptions> opts)
    {
        var o = opts.Value;
        _baseUrl = o.BaseUrl;
        var httpClientHandler = new HttpClientHandler
        {
            Credentials = new NetworkCredential(o.Username, o.Password)
        };
        var http = new HttpClient(httpClientHandler)
        {
            BaseAddress = new Uri(o.BaseUrl)
        };
        http.DefaultRequestHeaders.Add("OCS-APIRequest", "true");

        var clientParams = new WebDavClientParams
        {
            BaseAddress = new Uri(o.BaseUrl),
            Credentials = new NetworkCredential(o.Username, o.Password)
        };
        _client = new WebDavClient(clientParams);
    }

    public async Task<bool> UploadAsync(Stream fileStream, string filename, string? folder = null)
    {
        string remotePath = string.IsNullOrEmpty(folder)
            ? filename
            : $"{folder.TrimEnd('/')}/{filename}";

        var result = await _client.PutFile(remotePath, fileStream);

        Console.WriteLine($"[WebDAV] PUT {remotePath} → {(int)result.StatusCode} {result.StatusCode}");
        Console.WriteLine($"IsSuccessful: {result.IsSuccessful}");
        Console.WriteLine($"Description: {result.Description}");

        return result.IsSuccessful;
    }
}

