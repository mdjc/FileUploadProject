namespace FileUploadApi.Common;

public static class AllowedFileTypes
{
    public static readonly Dictionary<AllowedFileType, string> MimeTypes = new()
    {
        { AllowedFileType.Pdf,  "application/pdf" },
        { AllowedFileType.Docx, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { AllowedFileType.Jpg,  "image/jpeg" },
        { AllowedFileType.Png,  "image/png" },
        { AllowedFileType.Txt,  "text/plain" }
    };

    public static bool IsSupported(string? contentType)
    {
        return MimeTypes.Values.Contains(contentType);
    }

    public static string GetAllowedFileTypesList()
    {
        return string.Join(", ", MimeTypes.Keys.Select(t => t.ToString().ToUpper()));
    }
}
