namespace GalaFamilyLibrary.Infrastructure.Domains;

[SugarTable]
public abstract class EntityBase<T> : IEntityBase<T> where T : class, IEquatable<T>
{
    public bool IsDeleted { get; set; }

    public T Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Name { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public T CreatorId { get; set; }

    public T? UpdaterId { get; set; }
}