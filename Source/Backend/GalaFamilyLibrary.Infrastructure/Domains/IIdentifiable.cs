using MongoDB.Bson.Serialization.Attributes;

namespace GalaFamilyLibrary.Infrastructure.Domains;

public interface IIdentifiable<TKey> where TKey : IEquatable<TKey>
{
    [BsonId] [JsonProperty("_id")] TKey Id { get; set; }
    string Name { get; set; }
}