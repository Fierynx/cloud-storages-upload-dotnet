using Microsoft.AspNetCore.Mvc;

namespace FileCloudUpload
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileCloudService _fileCloud;

        public FileController(IFileCloudService fileCloud) => _fileCloud = fileCloud;

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string remoteFolder)
        {
            // Validate input parameters
            if (file == null || file.Length == 0) return BadRequest("File is missing.");

            try
            {
                // Upload the file to the specified remote folder
                using var stream = file.OpenReadStream();
                await _fileCloud.UploadAsync(stream, remoteFolder, file.FileName);
                return Ok($"Uploaded '{file.FileName}' to '{remoteFolder}'.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Upload failed: {ex.Message}");
            }
        }

    }

}