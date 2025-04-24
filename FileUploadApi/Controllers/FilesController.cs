using FileUploadApi.DTOs;
using FileUploadApi.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FileUploadApi.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(
        Summary = "Upload a file",
        Description = "Uploads a file to the server."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] FileUploadDto uploadDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); // structured validation error response

        var id = await _fileService.SaveFileAsync(uploadDto.File);
        return Ok(new { Id = id });
    }

    [HttpGet("metadata/{id}")]
    [SwaggerOperation(Summary = "Get file metadata by ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns metadata of the file", typeof(FileMetadataDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileMetadataDto>> GetMetadata(int id)
    {
        var metadata = await _fileService.GetFileMetadataByIdAsync(id);
        return Ok(metadata);
    }

    [HttpGet("download/{id}")]
    [SwaggerOperation(Summary = "Download a file by its ID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Download(int id)
    {
        var file = await _fileService.GetFileByIdAsync(id);

        return File(file.Data, file.ContentType, file.FileName);
    }
}
