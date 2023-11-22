namespace Cts.AppServices.Staff;

/// <summary>
/// The exception that is thrown if the current user can't be found.
/// </summary>
public class CurrentUserNotFoundException() : Exception("Information on the current user could not be found.");
