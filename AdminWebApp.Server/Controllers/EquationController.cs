using Microsoft.AspNetCore.Mvc;
using AdminWebApp.Server.Models;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminWebApp.Server.Models.MongoDB;

namespace AdminWebApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquationController : ControllerBase
    {
        private readonly ILogger<BusInfoController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public EquationController(ILogger<BusInfoController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public  ActionResult<IEnumerable<EquationDoc>> Equationinfo()
        {
            List<EquationDoc> lis = new List<EquationDoc>();
            EquationDoc test = new EquationDoc();
            test.Id = "1";
            test.Name = "公式名稱";
            test.Equation = "公式";
            lis.Add(test);
            return Ok(lis);
        }

    }
}
