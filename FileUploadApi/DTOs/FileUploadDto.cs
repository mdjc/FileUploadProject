using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace FileUploadApi.DTOs;

[SwaggerSchema("Represents a file upload request.")]
public class FileUploadDto
{
    [Required(ErrorMessage = "A file must be provided.")]
    [SwaggerSchema("The file to upload. Allowed types: PDF, DOCX, JPG, PNG, TXT. Max size: 10 MB.")]
    public IFormFile File { get; set; } = null!;
}
