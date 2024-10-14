using Microsoft.AspNetCore.Mvc;
using AdminWebApp.Server.Models;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminWebApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusInfoController : ControllerBase
    {
        private readonly ILogger<BusInfoController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public BusInfoController(ILogger<BusInfoController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBusInfo()
        {
            var routeIds = new Dictionary<string, int>
            {
                { "706", 706 },   // 706 路線（包括上班和下班）
                { "goBackB", 7062 }  // 下班B
            };

            var allBuses = new List<Bus>();            

            foreach (var route in routeIds)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://ebus.tycg.gov.tw/ebus/graphql");

                var query = new
                {
                    operationName = "QUERY_ESTIMATE_TIMES",
                    variables = new
                    {
                        lang = "zh",
                        routeId = route.Value
                    },
                    query = @"query QUERY_ESTIMATE_TIMES($routeId: Int!, $lang: String!) {
                        route(xno: $routeId, lang: $lang) {
                            id
                            departure
                            destination
                            buses {
                                ...busesInEstimateTimeFragment
                                __typename
                            }
                            estimateTimes {
                                ...estimateTimesFragment
                                __typename
                            }
                            stations {
                                ...routeStationsFragment
                                __typename
                            }
                            __typename
                        }
                      }

                      fragment busesInEstimateTimeFragment on RouteBusConnection {
                        edges {
                            node {
                                id
                                type
                                __typename
                            }
                            __typename
                        }
                        __typename
                      }

                      fragment routeStationsFragment on RouteStationConnection {
                        edges {
                            node {
                                id
                                name
                                __typename
                            }
                            __typename
                        }
                        __typename
                      }

                      fragment estimateTimesFragment on EstimateTimeConnection {
                        edges {
                            node {
                                id
                                goBack
                                comeTime
                                isSuspended
                                isConstruction
                                isEvent
                                isOperationDay
                                etas {
                                    busId
                                    etaTime
                                    isLast
                                    __typename
                                }
                                __typename
                            }
                            __typename
                        }
                        __typename
                      }"
                };

                var content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
                request.Content = content;

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResponse);

                    var estimateEdges = (JArray)jsonObject["data"]["route"]["estimateTimes"]["edges"];
                    var stationEdges = (JArray)jsonObject["data"]["route"]["stations"]["edges"];

                    var stationDictionary = new Dictionary<string, string>();

                    foreach (var station in stationEdges)
                    {
                        var stationId = station["node"]["id"].ToString();
                        var stationName = station["node"]["name"].ToString();

                        if (!stationDictionary.ContainsKey(stationId))
                        {
                            stationDictionary.Add(stationId, stationName);
                        }
                    }

                    foreach (var edge in estimateEdges)
                    {
                        var node = edge["node"] as JObject;
                        var stationId = node["id"].ToString();
                        var stationName = stationDictionary.ContainsKey(stationId) ? stationDictionary[stationId] : "Unknown Station";
                        var arrivalTime = node["comeTime"]?.ToString() ?? "未知";
                        var color = DetermineColor(node);
                        var goBack = node["goBack"]?.Value<int>() ?? 0;
                        var type = DetermineType(route.Key, goBack);

                        allBuses.Add(new Bus
                        {           
                            seq=0,
                            station = stationName,
                            arrivaltime = arrivalTime,
                            color = color,
                            type = type
                        });
                    }
                }
            }

            // 按類型分組並重新編排序號
            var groupedBuses = allBuses.GroupBy(b => b.type)
                                       .SelectMany(g => g.Select((bus, index) => new Bus
                                       {
                                           seq = index + 1,
                                           station = bus.station,
                                           arrivaltime = bus.arrivaltime,
                                           color = bus.color,
                                           type = bus.type
                                       }))
                                       .ToList();

            return Ok(groupedBuses);
        }

        private string DetermineColor(JObject node)
        {
            if (node["isSuspended"]?.Value<bool>() == true) return "red";
            if (node["isConstruction"]?.Value<bool>() == true) return "orange";
            if (node["isEvent"]?.Value<bool>() == true) return "yellow";
            if (node["isOperationDay"]?.Value<bool>() != true) return "gray";
            return "green";
        }

        private string DetermineType(string routeKey, int goBack)
        {
            if (routeKey == "706")
            {
                return goBack == 1 ? "上班" : "下班";
            }
            else if (routeKey == "goBackB")
            {
                return "下班B";
            }
            return "未知";
        }
    }
}
