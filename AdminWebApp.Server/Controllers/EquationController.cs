using Microsoft.AspNetCore.Mvc;
using AdminWebApp.Server.Models;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminWebApp.Server.Models.MongoDB;
using AdminWebApp.Server.Service;

namespace AdminWebApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquationController : ControllerBase
    {
        private readonly ILogger<BusInfoController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MongoDbService _mongoDbService;
        public EquationController(ILogger<BusInfoController> logger, IHttpClientFactory clientFactory, MongoDbService mongoDbService)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _mongoDbService = mongoDbService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquationDoc>>> Equationinfo()
        {
            var equations = await _mongoDbService.GetAllEquationsAsync();
            return Ok(equations);            
        }
        [HttpPost]
        public async Task<IActionResult> SaveEquation([FromBody] Equationinfo doc)
        {
            var equation = new EquationDoc
            {
                Name = doc.name,
                Equation = doc.equation
            };

            await _mongoDbService.InsertEquationAsync(equation);
            return Ok(new { message = "Equation saved successfully!" });
        }

    }
}
