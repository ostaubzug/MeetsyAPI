using MeetsyAPI;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseCors("CorsPolicy");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


MongoClientSettings settings =
    MongoClientSettings.FromConnectionString(
        "mongodb://admin:fajsdfi-asdifa4-ajfaknv-ckkdd@mongo:27017/");
MongoClient client = new MongoClient(settings);

IMongoCollection<MessageBubbleData> messageCollection = client.GetDatabase("DB1").GetCollection<MessageBubbleData>("MessageBubbleData");
IMongoCollection<ProposedMessageBubbleData> proposedMessageCollection = client.GetDatabase("DB1").GetCollection<ProposedMessageBubbleData>("ProposedMessageBubbleData");


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

app.MapPost("/addProposedMessageBubbleData", async (ProposedMessageBubbleData proposedMessageBubbleData) =>
    {
        try
        {
            proposedMessageBubbleData.TimeStamp = DateTimeOffset.UtcNow;
            await proposedMessageCollection.InsertOneAsync(proposedMessageBubbleData);
            return Results.StatusCode(100);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                title: "Failed to add message bubble data",
                detail: ex.Message,
                statusCode: 500
            );
        }
    }).WithName("AddProposedMessageBubbleData")
    .WithOpenApi();


static async Task<List<MessageBubbleData>> GetAllMessageBubbleData(IMongoCollection<MessageBubbleData> messageCollection)
{
    var messageCursor = await messageCollection.FindAsync(new MongoDB.Bson.BsonDocument());
    return await messageCursor.ToListAsync();
}

app.Run();