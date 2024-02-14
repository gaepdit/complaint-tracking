namespace Cts.AppServices.Attachments.ValidationAttributes;

public class ValidateFilesResult
{
    /// <summary>
    /// Flag indicating whether if the files were successfully validated or not.
    /// </summary>
    /// <value>True if validation succeeded, otherwise false.</value>
    public bool IsValid { get; private init; }

    /// <summary>
    /// A <see cref="List{T}"/> of <see cref="string"/> containing errors that occurred during the operation.
    /// </summary>
    /// <value>A <see cref="List{T}"/> of <see cref="string"/> instances.</value>
    public List<string> ValidationErrors { get; } = [];

    /// <summary>
    /// Returns a <see cref="ValidateFilesResult"/> indicating successful validation.
    /// </summary>
    /// <returns>A <see cref="ValidateFilesResult"/> indicating successful validation.</returns>
    public static ValidateFilesResult Valid => new() { IsValid = true };

    /// <summary>
    /// Returns a <see cref="ValidateFilesResult"/> indicating unsuccessful validation.
    /// </summary>
    /// <param name="validationErrors">The validation errors encountered.</param>
    /// <returns>A <see cref="ValidateFilesResult"/> indicating unsuccessful validation.</returns>
    public static ValidateFilesResult Invalid(IEnumerable<string> validationErrors)
    {
        var result = new ValidateFilesResult { IsValid = false };
        result.ValidationErrors.AddRange(validationErrors);
        return result;
    }
}
