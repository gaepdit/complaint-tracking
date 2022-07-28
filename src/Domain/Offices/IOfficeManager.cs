using Cts.Domain.Users;
using System.Runtime.Serialization;

namespace Cts.Domain.Offices;

public interface IOfficeManager
{
    Task<Office> CreateAsync(string name, ApplicationUser? user = null);
    Task ChangeNameAsync(Office office, string name);
}

/// <summary>
/// The exception that is thrown if an office is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class OfficeAlreadyExistsException : Exception
{
    public OfficeAlreadyExistsException(string name)
        : base($"An Office with that name already exists. Name: {name}") { }

    protected OfficeAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
