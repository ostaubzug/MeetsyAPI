namespace MeetsyAPI;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class MessageBubbleData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("ort")] public string? Location { get; set; }

    [BsonElement("einladungstext")] public string? Description { get; set; }

    [BsonElement("zeitpunkt")] public string? Time { get; set; }
}