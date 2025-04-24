using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FileUploadApi.Entities;
using FileUploadApi.DTOs;

namespace FileUploadApi.Services;

public interface IFileService
{
    Task<int> SaveFileAsync(IFormFile file);
    Task<FileEntity> GetFileByIdAsync(int id);
    Task<FileMetadataDto> GetFileMetadataByIdAsync(int id);
}
