using Swashbuckle.AspNetCore.Annotations;

namespace FileUploadApi.DTOs;

[SwaggerSchema(Description = "Metadata about a stored file.")]
public class FileMetadataDto
{
    [SwaggerSchema("File ID")]
    public int Id { get; set; }

    [SwaggerSchema("Original filename")]
    public string FileName { get; set; } = string.Empty;

    [SwaggerSchema("MIME type")]
    public string ContentType { get; set; } = string.Empty;

    [SwaggerSchema("Size in bytes")]
    public long Size { get; set; }

    [SwaggerSchema("Timestamp when the file was uploaded (UTC).")]
    public DateTime UploadedAt { get; set; }
}

