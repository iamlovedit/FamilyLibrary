namespace GalaFamilyLibrary.Infrastructure.Domains;

public abstract class IdentifiableBase<TKey> : IIdentifiable<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }

    public string Name { get; set; }
}