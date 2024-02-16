using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using HttpClientToCurl;

namespace BoostStudio.Application.Publishing.Commands;

public record PublishRemote(string Path) : IRequest;

public class PublishRemoteHandler(IHttpClientFactory httpClientFactory) : IRequestHandler<PublishRemote>
{
    private const string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwicHdkX3RzIjowLCJleHAiOjE3MDYzNjQ4ODksIm5iZiI6MTcwNjE5MjA4OSwiaWF0IjoxNzA2MTkyMDg5fQ.IKrQIpOZsqCuWEjqQTAomf0J_8eeYvPC0I5ajTydjMI";

    public async Task Handle(PublishRemote command, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("IgnoreSsl");
        var fileSystemEntries = Directory.GetFileSystemEntries(command.Path, "*", SearchOption.AllDirectories);
        foreach (var entry in fileSystemEntries)
        {
            var fileAttributes = File.GetAttributes(entry);
            var relativePath = Path.GetRelativePath(command.Path, entry);
            if (fileAttributes == FileAttributes.Directory)
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5244/api/fs/mkdir");
                httpRequestMessage.Content = JsonContent.Create(new
                {
                    path = $"/Ctfile/{relativePath}"
                }, options: new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                httpRequestMessage.Headers.Add("Authorization", Token);
                var responseMessage = await client.SendAsync(httpRequestMessage, cancellationToken);
                Console.WriteLine(await responseMessage.Content.ReadAsStringAsync(cancellationToken));
            }
            else
            {
                if (!File.Exists(entry))
                    continue;

                var fileName = Path.GetFileName(entry);
                await using var file = File.OpenRead(entry);
                using MultipartFormDataContent multipartContent = new();
                var fileContent = new StreamContent(file);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Multipart.FormData);
                multipartContent.Add(fileContent, "file", fileName);

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "http://localhost:5244/api/fs/form");
                httpRequestMessage.Content = multipartContent;
                httpRequestMessage.Headers.Add("Authorization", Token);
                httpRequestMessage.Headers.Add("File-Path", $"Ctfile/{relativePath}");
                var responseMessage = await client.SendAsync(httpRequestMessage, cancellationToken);
            }
        }
    }
}
