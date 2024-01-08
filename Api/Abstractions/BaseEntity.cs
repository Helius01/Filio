using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Filio.Api.Abstractions;

/// <summary>
/// Base Entity
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Primary Key
    /// </summary>
    /// <returns></returns>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// CreatedAt
    /// </summary>
    /// <value></value>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// UpdatedAt
    /// </summary>
    /// <value></value>
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// Returns the entity dto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected abstract T ToDto<T>() where T : IDto;

    /// <summary>
    /// Applies effects on create mode
    /// </summary>
    public void OnBaseCreate()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Applies effects on update mode
    /// </summary>
    public void OnBaseUpdate()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}