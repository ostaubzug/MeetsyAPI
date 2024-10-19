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
    
    //evt. zuerst auf C# Objekte mappen und dann wieder nach json serialisieren - LINQ kann auch genutzt werden so

    // Fetch all events asynchronously and map them to Event objects
    List<Event> events = await GetAllEvents(eventsCollection);
    return Results.Json(events); 
}).WithName("GetAllEvents").WithOpenApi();

static async Task<List<Event>> GetAllEvents(IMongoCollection<Event> eventsCollection)
{
    // Fetch all documents in the collection
    var eventsCursor = await eventsCollection.FindAsync(new MongoDB.Bson.BsonDocument());
    return await eventsCursor.ToListAsync();  // Return the list of mapped Event objects
}

/*app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();*/

app.Run();