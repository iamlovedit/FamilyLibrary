namespace GalaFamilyLibrary.Infrastructure.Domains;

public interface IPrimaryKey<T> where T : IEquatable<T>
{
    [SugarColumn(IsPrimaryKey = true)] public T Id { get; set; }
}