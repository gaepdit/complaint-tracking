using System.Runtime.Serialization;

namespace Cts.Domain.Exceptions;

/// <summary>
/// The exception that is thrown if an attachment is created with an invalid file type.
/// </summary>
[Serializable]
public class InvalidFileTypeException : Exception
{
    public InvalidFileTypeException(string fileType) : base($"The file type {fileType} is invalid.") { }
    protected InvalidFileTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
