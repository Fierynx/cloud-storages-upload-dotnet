using System;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DropboxUpload
{
  internal class Program
  {
    static async Task Main(string[] args)
    {
      // Load configuration from appsettings.json
      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

      string? AccessToken = config["Dropbox:AccessToken"];

      if (string.IsNullOrEmpty(AccessToken))
      {
        Console.WriteLine("Error: Dropbox access token not found in config.");
        return;
      }

      // Initialize Dropbox client with the access token
      using var dbx_client = new DropboxClient(AccessToken);

      string fileToUpload = "./dropbox-test-image.png";
      string folder = "/newfolder/folder-dalam-folder";
      string filename = "test-image.png";
      string targetPath = $"{folder}/{filename}";

      try
      {
        // Upload the file to Dropbox
        using var fs = File.Open(fileToUpload, FileMode.Open);
        var updated = await dbx_client.Files.UploadAsync(
            targetPath,
            WriteMode.Overwrite.Instance,
            body: fs
        );

        Console.WriteLine("File uploaded to Dropbox: " + updated.PathDisplay);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error uploading file: " + ex.Message);
      }

    }
  }
}