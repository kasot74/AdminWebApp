using Microsoft.AspNetCore.Mvc;
using AdminWebApp.Server.Models.MongoDB;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdminWebApp.Server.Service;

namespace AdminWebApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;        
        private readonly MongoDbService _mongoDbService;
        private readonly UserRepository _userRepository;

        public UsersController(ILogger<UsersController> logger, IConfiguration configuration, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _userRepository = new UserRepository(mongoDbService);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {            
            var users = await _userRepository.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var users = await _userRepository.GetAllUsersAsync();
            var user = users.Find(u => u.Id == id);
            return Ok(user);
        }
        
        public class NewUser {
            public string username { get; set; }
            public string password { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] NewUser x)
        {
            await _userRepository.CreateUserAsync(x.username, x.password);
            var users = await _userRepository.GetAllUsersAsync();
            var user = users.Find(u => u.Id == x.username);
            return Ok(user);
            
        }


    }
}