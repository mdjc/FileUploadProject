using Xunit;
using Moq;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using FileUploadApi.Data;
using FileUploadApi.Services;
using FileUploadApi.Entities;
using FileUploadApi.Exceptions;

namespace FileUploadApi.Tests.Services;

public class FileServiceTests
{
    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static IFormFile CreateFakeFormFile(string name, string content, string contentType = "text/plain")
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", name)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    [Fact]
    public async Task SaveFileAsync_ShouldStoreFileCorrectly()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();
        var logger = new Mock<ILogger<FileService>>();
        var service = new FileService(dbContext, logger.Object);

        var file = CreateFakeFormFile("test.txt", "hello world");

        // Act
        var id = await service.SaveFileAsync(file);

        // Assert
        var saved = await dbContext.Files.FindAsync(id);
        saved.Should().NotBeNull();
        saved!.FileName.Should().Be("test.txt");
        Encoding.UTF8.GetString(saved.Data).Should().Be("hello world");
    }

    [Fact]
    public async Task GetFileMetadataByIdAsync_ShouldReturnMetadata()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();
        var logger = new Mock<ILogger<FileService>>();
        var service = new FileService(dbContext, logger.Object);

        var entity = new FileEntity
        {
            FileName = "data.png",
            ContentType = "image/png",
            Data = new byte[] { 1, 2, 3 },
            UploadedAt = DateTime.UtcNow
        };

        dbContext.Files.Add(entity);
        await dbContext.SaveChangesAsync();

        // Act
        var dto = await service.GetFileMetadataByIdAsync(entity.Id);

        // Assert
        dto.FileName.Should().Be("data.png");
        dto.ContentType.Should().Be("image/png");
        dto.Size.Should().Be(3);
        dto.UploadedAt.Should().BeCloseTo(entity.UploadedAt, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetFileMetadataByIdAsync_ShouldThrow_WhenNotFound()
    {
        var service = new FileService(CreateInMemoryDbContext(), new Mock<ILogger<FileService>>().Object);

        Func<Task> act = async () => await service.GetFileMetadataByIdAsync(999);

        await act.Should().ThrowAsync<FileNotFoundInRepositoryException>()
            .WithMessage("*999*");
    }

    [Fact]
    public async Task GetFileByIdAsync_ShouldReturnFile_WhenExists()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();
        var logger = new Mock<ILogger<FileService>>();
        var service = new FileService(dbContext, logger.Object);

        var entity = new FileEntity
        {
            FileName = "invoice.pdf",
            ContentType = "application/pdf",
            Data = Encoding.UTF8.GetBytes("test"),
            UploadedAt = DateTime.UtcNow
        };

        dbContext.Files.Add(entity);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetFileByIdAsync(entity.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.FileName.Should().Be("invoice.pdf");
        result.ContentType.Should().Be("application/pdf");
        result.Data.Should().BeEquivalentTo(entity.Data);
    }


    [Fact]
    public async Task GetFileByIdAsync_ShouldThrow_WhenNotFound()
    {
        var service = new FileService(CreateInMemoryDbContext(), new Mock<ILogger<FileService>>().Object);

        Func<Task> act = async () => await service.GetFileByIdAsync(999);

        await act.Should().ThrowAsync<FileNotFoundInRepositoryException>();
    }
}
