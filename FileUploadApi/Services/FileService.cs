using FileUploadApi.Data;
using FileUploadApi.Entities;
using FileUploadApi.Exceptions;
using FileUploadApi.DTOs;
using Microsoft.AspNetCore.Http;

namespace FileUploadApi.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly AppDbContext _dbContext;

    public FileService(AppDbContext dbContext, ILogger<FileService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<FileEntity> GetFileByIdAsync(int id)
    {
        var file = await _dbContext.Files.FindAsync(id);

        if (file == null)
            throw new FileNotFoundInRepositoryException(id);

        return file;
    }

    public async Task<FileMetadataDto> GetFileMetadataByIdAsync(int id)
    {
        var file = await _dbContext.Files.FindAsync(id);

        if (file == null)
            throw new FileNotFoundInRepositoryException(id);

        return new FileMetadataDto
        {
            Id = file.Id,
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Data.Length,
            UploadedAt = file.UploadedAt
        };
    }


    public async Task<int> SaveFileAsync(IFormFile file)
    {
        _logger.LogDebug("Start SaveFileAsync: FileName={FileName}, ContentType={ContentType}, Size={Size} bytes",
            file.FileName, file.ContentType, file.Length);

        try
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var entity = new FileEntity
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Data = memoryStream.ToArray()
            };

            _dbContext.Files.Add(entity);
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug("Completed SaveFileAsync: FileName={FileName}, ID={FileId}", file.FileName, entity.Id);

            return entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SaveFileAsync while saving file: {FileName}", file.FileName);
            throw;
        }
    }
}
