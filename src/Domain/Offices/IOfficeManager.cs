using Cts.Domain.Entities;
using Cts.Domain.Users;
using System.Runtime.Serialization;

namespace Cts.Domain.Offices;

public interface IOfficeManager
{
    Task<Office> CreateAsync(string name, ApplicationUser? user = null, CancellationToken token = default);
    Task ChangeNameAsync(Office office, string name, CancellationToken token = default);
}

/// <summary>
/// The exception that is thrown if an office is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class OfficeNameAlreadyExistsException : Exception
{
    public OfficeNameAlreadyExistsException(string name)
        : base($"An Office with that name already exists. Name: {name}") { }

    protected OfficeNameAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
