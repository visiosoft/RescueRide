using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RescueRide.Data;

namespace RescueRide.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IOptions<MongoSettings> mongoSettings)
        {
            
            var settings = MongoClientSettings.FromConnectionString(mongoSettings.Value.ConnectionString);
            settings.ConnectTimeout = TimeSpan.FromSeconds(60);  // Increase connection timeout
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(60);  // Increase server selection timeout
            var client = new MongoClient(settings);
            _database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        }

        // This method correctly uses a generic type parameter for the collection
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);  // Ensure that T is used here
        }

        // Method to get a collection (replace YourModel with the actual model type)
     
        // Method to insert a document
        public async Task InsertDocumentAsync<T>(T document, string collectionName)
        {
            var collection = GetCollection<T>(collectionName);
            Console.WriteLine($"Inserting document to collection {collectionName}...");
            await collection.InsertOneAsync(document);
            Console.WriteLine("Document inserted successfully.");
        }
    }

    // Sample model class (replace this with your own model)
    public class YourModel
    {
        public string Id { get; set; }  // The MongoDB ID field
        public string Name { get; set; }
        // Add other properties here
    }
}
