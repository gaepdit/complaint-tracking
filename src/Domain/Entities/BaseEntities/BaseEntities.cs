namespace Cts.Domain.Entities.BaseEntities;

// This file contains slightly more specific implementations of AuditableEntity and AuditableSoftDeleteEntity
// with default data types for Entity and User primary keys.

/// <summary>
/// The default implementation of <see cref="AuditableEntity{TKey}"/> using Guid as the Entity primary key.
/// </summary>
public abstract class AuditableEntity : AuditableEntity<Guid>
{
    protected AuditableEntity() { }
    protected AuditableEntity(Guid id) : base(id) { }
}

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="IAuditableEntity{TUserKey}"/>.
/// A <see cref="string"/> is used for the User primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class AuditableEntity<TKey> : AuditableEntity<TKey, string>
    where TKey : IEquatable<TKey>
{
    protected AuditableEntity() { }
    protected AuditableEntity(TKey id) : base(id) { }
}

/// <summary>
/// The default implementation of <see cref="AuditableSoftDeleteEntity{TKey}"/> using Guid as the Entity primary key.
/// </summary>
public abstract class AuditableSoftDeleteEntity : AuditableSoftDeleteEntity<Guid> { }

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="IAuditableEntity{TUserKey}"/>
/// and <see cref="ISoftDelete{TUserKey}"/>. A <see cref="string"/> is used for the User primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class AuditableSoftDeleteEntity<TKey> : AuditableSoftDeleteEntity<TKey, string>
    where TKey : IEquatable<TKey>
{
    protected AuditableSoftDeleteEntity() { }
    protected AuditableSoftDeleteEntity(TKey id) : base(id) { }
}
