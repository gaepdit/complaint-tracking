namespace Cts.AppServices.Staff;

/// <summary>
/// The exception that is thrown if the current user does not have the required permission.
/// </summary>
public class InsufficientPermissionsException(string permission)
    : Exception($"The {permission} permission is needed to perform this operation.");
