using Microsoft.AspNetCore.Mvc;
using AdminWebApp.Server.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdminWebApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly string _connectionString;

        public UsersController(ILogger<UsersController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUsers()
        {
            var users = new List<UserAccount>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SELECT UserID, Username, CreatedAt, LastLoginAt, IsActive FROM UserAccounts", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new UserAccount
                            {
                                Userid = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                CreatedAt = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                                LastLoginAt = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                IsActive = reader.IsDBNull(4) ? (bool?)null : reader.GetBoolean(4)
                            });
                        }
                    }
                }
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount>> GetUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SELECT UserID, Username, CreatedAt, LastLoginAt, IsActive FROM UserAccounts WHERE UserID = @UserID", connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return Ok(new UserAccount
                            {
                                Userid = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                CreatedAt = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                                LastLoginAt = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                IsActive = reader.IsDBNull(4) ? (bool?)null : reader.GetBoolean(4)
                            });
                        }
                    }
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var salt = GenerateSalt();
                var passwordHash = ComputeHash(user.Password, salt);
                using (var command = new SqlCommand("INSERT INTO UserAccounts (Username, PasswordHash, PasswordSalt, CreatedAt) VALUES (@Username, @PasswordHash, @PasswordSalt, @CreatedAt); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", salt);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);                    
                    var newUserId = await command.ExecuteScalarAsync();
                    return CreatedAtAction(nameof(GetUser), new { id = newUserId }, user);
                }
            }            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserAccount user)
        {
            if (id != user.Userid) return BadRequest();
            var salt = GenerateSalt();
            var passwordHash = ComputeHash(user.Password, salt);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UPDATE UserAccounts SET Username = @Username, PasswordHash = @PasswordHash,PasswordSalt = @PasswordSalt WHERE UserID = @UserID", connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", salt);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0) return NotFound();
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("DELETE FROM UserAccounts WHERE UserID = @UserID", connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0) return NotFound();
                }
            }
            return NoContent();
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
}