using Microsoft.AspNetCore.Mvc;

namespace NextCloudUpload
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly INextCloudService _nc;

        public FileController(INextCloudService nc) => _nc = nc;

        [HttpPost("upload")]
        public async Task<IActionResult> Post(IFormFile file, [FromForm] string? folder)
        {
            if (file is null || file.Length == 0)
                return BadRequest("No file provided.");

            await using var stream = file.OpenReadStream();
            var ok = await _nc.UploadAsync(stream, file.FileName, folder);

            if (!ok)
                return StatusCode(500, "Upload to Nextcloud failed.");

            return Ok($"Uploaded '{file.FileName}' to '{folder}'.");
        }
    }
}
