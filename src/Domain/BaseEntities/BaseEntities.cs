namespace Cts.Domain.BaseEntities;

/// <summary>
/// The default implementation of <see cref="AuditableEntity{TKey}"/> using Guid as the primary key.
/// </summary>
public abstract class AuditableEntity : AuditableEntity<Guid>
{
    protected AuditableEntity() { }
    protected AuditableEntity(Guid id) : base(id) { }
}

/// <summary>
/// The default <see cref="IEntity{TKey}"/> implementation for the project, which also implements
/// <see cref="IAuditableEntity{TUserKey}"/>. A <see cref="string"/> is used for the User primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class AuditableEntity<TKey> : AuditableEntity<TKey, string>
    where TKey : IEquatable<TKey>
{
    protected AuditableEntity() { }
    protected AuditableEntity(TKey id) : base(id) { }
}

/// <summary>
/// The default implementation of <see cref="SoftDeleteEntity{TKey}"/> using Guid as the primary key.
/// </summary>
public abstract class SoftDeleteEntity : SoftDeleteEntity<Guid> { }

/// <summary>
/// An <see cref="IEntity{TKey}"/> implementation for this project, which also implements
/// <see cref="IAuditableEntity{TUserKey}"/> and <see cref="ISoftDelete{TUserKey}"/>.
/// A <see cref="string"/> is used for the User primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class SoftDeleteEntity<TKey> : AuditableEntity<TKey>, ISoftDelete<string>
    where TKey : IEquatable<TKey>
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    protected SoftDeleteEntity() { }
    protected SoftDeleteEntity(TKey id) : base(id) { }
}
