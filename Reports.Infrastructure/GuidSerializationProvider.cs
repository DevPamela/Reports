using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Reports.Infrastructure;

internal class GuidSerializationProvider : IBsonSerializationProvider
{
    public IBsonSerializer? GetSerializer(Type type) =>
        type == typeof(Guid) ? new GuidSerializer(GuidRepresentation.Standard) : null;
}