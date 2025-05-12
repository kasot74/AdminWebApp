using AdminWebApp.Server.Models.MongoDB;
using MongoDB.Driver;


namespace AdminWebApp.Server.Service
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<EquationDoc> _equationCollection;
        public MongoDbService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("myDatabase");
            _equationCollection = _database.GetCollection<EquationDoc>("Equations");
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        
        public async Task InsertEquationAsync(EquationDoc equation)
        {
            await _equationCollection.InsertOneAsync(equation);
        }

        public async Task<List<EquationDoc>> GetAllEquationsAsync()
        {
            return await _equationCollection.Find(_ => true).ToListAsync();
        }

    }
}
