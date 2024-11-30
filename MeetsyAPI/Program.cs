using MeetsyAPI;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Allows requests from local development
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


MongoClientSettings settings =
    MongoClientSettings.FromConnectionString(
        "mongodb://admin:fajsdfi-asdifa4-ajfaknv-ckkdd@meetsy-testsrv.prod.projects.ls.eee.intern:27017/");
MongoClient client = new MongoClient(settings);

IMongoCollection<Event> eventsCollection = client.GetDatabase("DB1").GetCollection<Event>("Events");
IMongoCollection<MessageBubbleData> messageCollection = client.GetDatabase("DB1").GetCollection<MessageBubbleData>("MessageBubbleData");


app.MapGet("/getAllEvents",  async() =>
{
    List<Event> events = await GetAllEvents(eventsCollection);
    return Results.Json(events); 
}).WithName("GetAllEvents").WithOpenApi();

app.MapGet("/getAllMessageBubbleData",  async() =>
{
    List<MessageBubbleData> messages = await GetAllMessageBubbleData(messageCollection);
    return Results.Json(messages); 
}).WithName("GetAllMessageBubbleData").WithOpenApi();

app.MapPost("/addMessageBubbleData", async (MessageBubbleData messageBubbleData) =>
    {
        try
        {
            await messageCollection.InsertOneAsync(messageBubbleData);
            return Results.Created($"/getAllMessageBubbleData", messageBubbleData);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                title: "Failed to add message bubble data",
                detail: ex.Message,
                statusCode: 500
            );
        }
    }).WithName("AddMessageBubbleData")
    .WithOpenApi();

static async Task<List<Event>> GetAllEvents(IMongoCollection<Event> eventsCollection)
{
    var eventsCursor = await eventsCollection.FindAsync(new MongoDB.Bson.BsonDocument());
    return await eventsCursor.ToListAsync();
}

static async Task<List<MessageBubbleData>> GetAllMessageBubbleData(IMongoCollection<MessageBubbleData> messageCollection)
{
    var messageCursor = await messageCollection.FindAsync(new MongoDB.Bson.BsonDocument());
    return await messageCursor.ToListAsync();
}

app.Run();