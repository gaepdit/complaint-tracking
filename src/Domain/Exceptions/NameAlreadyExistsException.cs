using System.Runtime.Serialization;

namespace Cts.Domain.Exceptions;

/// <summary>
/// The exception that is thrown if a named entity is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class NameAlreadyExistsException : Exception
{
    public NameAlreadyExistsException(string name) : base($"An entity with that name already exists. Name: {name}") { }

    protected NameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
