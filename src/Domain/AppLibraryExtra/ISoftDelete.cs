namespace Cts.Domain.AppLibraryExtra;

/// <summary>
/// Marks an entity for enabling soft deletion in the database.
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAt { get; }
}
