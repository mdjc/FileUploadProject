namespace FileUploadApi.Exceptions;

public class FileNotFoundInRepositoryException : Exception
{
    public FileNotFoundInRepositoryException(int id)
        : base($"No file found in the repository with ID = {id}.") { }
}
