using MongoDB.Bson.Serialization.Attributes;

namespace GalaFamilyLibrary.Infrastructure.Domains;

public interface IIdentifiable<TKey> where TKey : IEquatable<TKey>
{
    [BsonId] TKey Id { get; set; }
    string Name { get; set; }
}