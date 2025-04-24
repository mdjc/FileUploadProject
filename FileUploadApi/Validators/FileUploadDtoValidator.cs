using FileUploadApi.DTOs;
using FluentValidation;
using FileUploadApi.Common;

namespace FileUploadApi.Validators;

public class FileUploadDtoValidator : AbstractValidator<FileUploadDto>
{
    private const long MaxFileSize = 10 * 1024 * 1024;

    public FileUploadDtoValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("A file must be provided.")
            .DependentRules(() =>
            {
                RuleFor(x => x.File)
                    .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
                    .Must(f => f.Length <= MaxFileSize)
                        .WithMessage("File size must be less than 10 MB.")
                    .Must(f => AllowedFileTypes.IsSupported(f.ContentType))
                        .WithMessage($"Only {AllowedFileTypes.GetAllowedFileTypesList()} files are allowed.");
            });
    }
}
