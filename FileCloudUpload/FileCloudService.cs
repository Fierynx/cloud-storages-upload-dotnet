using FileCloudUpload;
using Microsoft.Extensions.Options;

public interface IFileCloudService
{
    Task<bool> UploadAsync(Stream fileStream, string remoteFolder, string fileName);
}

public class FileCloudService : IFileCloudService
{
    private readonly HttpClient _client;
    private readonly FileCloudOptions _opt;

    public FileCloudService(HttpClient client, IOptions<FileCloudOptions> opt)
    {
        _client = client;
        _opt = opt.Value;
    }

    // Logs in to FileCloud and retrieves the session cookie.
    private async Task<string> LoginAsync()
    {
        var loginUrl = $"{_opt.BaseUrl}/core/loginguest";
        var content = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("userid", _opt.Username),
            new KeyValuePair<string, string>("password", _opt.Password)
        ]);

        var resp = await _client.PostAsync(loginUrl, content);
        resp.EnsureSuccessStatusCode();
        var cookie = resp.Headers.GetValues("Set-Cookie").FirstOrDefault() ?? "";
        return cookie;
    }

    public async Task<bool> UploadAsync(Stream fileStream, string remoteFolder, string fileName)
    {
        var cookie = await LoginAsync();

        // Required parameters for FileCloud upload API
        using var content = new MultipartFormDataContent
        {
            { new StringContent("explorer"), "appname" },
            { new StringContent(remoteFolder), "path" },
            { new StringContent("0"), "offset" },
            { new StringContent("1"), "complete" },
            { new StreamContent(fileStream), "file", fileName }
        };

        _client.DefaultRequestHeaders.Remove("Cookie");
        _client.DefaultRequestHeaders.Add("Cookie", cookie);

        var uploadUrl = $"{_opt.BaseUrl}/core/upload";
        var resp = await _client.PostAsync(uploadUrl, content);
        return resp.IsSuccessStatusCode;
    }
}
