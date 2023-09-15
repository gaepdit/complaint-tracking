using System.Runtime.Serialization;

namespace Cts.AppServices.Staff;

/// <summary>
/// The exception that is thrown if the current user can't be found.
/// </summary>
[Serializable]
public class CurrentUserNotFoundException : Exception
{
    public CurrentUserNotFoundException()
        : base("Information on the current user could not be found.") { }

    protected CurrentUserNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
