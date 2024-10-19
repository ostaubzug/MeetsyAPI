using MeetsyAPI;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapGet("/getAllEvents",  async() =>
{
    List<Event> events = await GetAllEvents(eventsCollection);
    return Results.Json(events); 
}).WithName("GetAllEvents").WithOpenApi();

static async Task<List<Event>> GetAllEvents(IMongoCollection<Event> eventsCollection)
{
    var eventsCursor = await eventsCollection.FindAsync(new MongoDB.Bson.BsonDocument());
    return await eventsCursor.ToListAsync();
}

app.Run();