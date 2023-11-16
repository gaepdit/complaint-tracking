namespace Cts.Domain.Exceptions;

/// <summary>
/// The exception that is thrown if an attachment is created with an invalid file type.
/// </summary>
public class InvalidFileTypeException(string fileType) : Exception($"The file type {fileType} is invalid.");
