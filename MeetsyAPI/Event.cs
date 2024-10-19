namespace MeetsyAPI;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Event
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("eventName")] public string? EventName { get; set; }

    [BsonElement("eventDescription")] public string? EventDescription { get; set; }

    [BsonElement("eventDate")] public string? EventDate { get; set; }

    [BsonElement("link")] public string? Link { get; set; }

    [BsonElement("image")] public string? Image { get; set; }

    [BsonElement("entryFee")] public string? EntryFee { get; set; }

    [BsonElement("ageLimit")] public string? AgeLimit { get; set; }

    [BsonElement("location")] public string? Location { get; set; }

    [BsonElement("time")] public string? Time { get; set; }
}