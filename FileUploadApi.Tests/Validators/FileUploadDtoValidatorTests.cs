using Xunit;
using FluentAssertions;
using FileUploadApi.DTOs;
using FileUploadApi.Validators;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace FileUploadApi.Tests.Validators;

public class FileUploadDtoValidatorTests
{
    private static IFormFile CreateFakeFile(string name, string content, string contentType = "application/pdf", long? overrideSize = null)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        return new FormFile(stream, 0, overrideSize ?? stream.Length, "file", name)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    [Theory]
    [InlineData(null, "A file must be provided.")]
    public void ShouldFail_WhenFileIsNullOrInvalid(IFormFile file, string expectedMessage)
    {
        var dto = new FileUploadDto { File = file };
        var validator = new FileUploadDtoValidator();

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
    }

    [Fact]
    public void ShouldFail_WhenFileIsEmpty()
    {
        var file = CreateFakeFile("empty.txt", "", "text/plain", 0);
        var dto = new FileUploadDto { File = file };
        var validator = new FileUploadDtoValidator();

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "File cannot be empty.");
    }

    [Fact]
    public void ShouldFail_WhenFileIsTooLarge()
    {
        var largeSize = 11 * 1024 * 1024; // 11MB
        var file = CreateFakeFile("bigfile.pdf", "x", "application/pdf", largeSize);
        var dto = new FileUploadDto { File = file };
        var validator = new FileUploadDtoValidator();

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "File size must be less than 10 MB.");
    }

    [Fact]
    public void ShouldFail_WhenFileTypeIsUnsupported()
    {
        var file = CreateFakeFile("malware.exe", "content", "application/x-msdownload");
        var dto = new FileUploadDto { File = file };
        var validator = new FileUploadDtoValidator();

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Only"));
    }

    [Fact]
    public void ShouldPass_WhenFileIsValid()
    {
        var file = CreateFakeFile("file.pdf", "hello", "application/pdf");
        var dto = new FileUploadDto { File = file };
        var validator = new FileUploadDtoValidator();

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
