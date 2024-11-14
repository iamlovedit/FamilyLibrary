namespace GalaFamilyLibrary.Infrastructure.Domains;

public interface IEntityBase<T> : IPrimaryKey<T>, IDateAbility,IOperateAbility<T>, IDeletable where T : IEquatable<T>
{
    public string? Name { get; set; }

}

public interface IOperateAbility<T> where T : IEquatable<T>
{
    T CreatorId { get; set; }

    T? UpdaterId { get; set; }
}