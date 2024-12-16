using MongoDB.Driver;
using MongoDB.Bson;
using AdminWebApp.Server.Models.MongoDB;
using System.Security.Cryptography;
using System.Text;
using AdminWebApp.Server.Service;

public class UserRepository
{

    private readonly MongoDbService _mongoDbService;
    private readonly IMongoCollection<User> _usersCollection;
    public UserRepository(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
        _usersCollection = mongoDbService.GetCollection<User>("UserAccounts");
        // 為 Username 欄位建立唯一索引
        var indexKeysDefinition = Builders<User>.IndexKeys.Ascending(u => u.Username);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<User>(indexKeysDefinition, indexOptions);
        _usersCollection.Indexes.CreateOne(indexModel);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _usersCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task UpdateUserAsync(string id, string password, bool isActive)
    {
        var salt = GenerateSalt();
        var passwordHash = ComputeHash(password, salt);

        var update = Builders<User>.Update
            .Set(u => u.IsActive, isActive)
            .Set(u => u.PasswordHash, passwordHash)
            .Set(u => u.PasswordSalt, salt);

        await _usersCollection.UpdateOneAsync(u => u.Id == id, update);
    }

    public async Task UpdateLastLoginAtAsync(string userId)
    {
        var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
        var taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);

        var update = Builders<User>.Update.Set(u => u.LastLoginAt, taiwanTime);
        await _usersCollection.UpdateOneAsync(u => u.Id == userId, update);
    }

    public async Task<User> ValidateUserAsync(string username, string password)
    {
        var user = await _usersCollection.Find(u => u.Username == username && u.IsActive == true).FirstOrDefaultAsync();

        if (user != null)
        {
            var passwordHash = ComputeHash(password, user.PasswordSalt);
            if (user.PasswordHash == passwordHash)
            {
                await UpdateLastLoginAtAsync(user.Id);
                return user;
            }
        }
        return null;
    }

    public async Task CreateUserAsync(string username, string password)
    {
        var salt = GenerateSalt();
        var passwordHash = ComputeHash(password, salt);
        var taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
        var taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanTimeZone);
        var newUser = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            IsActive = true,
            CreatedAt = taiwanTime
        };

        await _usersCollection.InsertOneAsync(newUser);
    }

    public async Task DeleteUserAsync(string id)
    {
        await _usersCollection.DeleteOneAsync(u => u.Id == id);
    }

    private string ComputeHash(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(saltedPassword);
            return Convert.ToBase64String(hash);
        }
    }

    private string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
}
