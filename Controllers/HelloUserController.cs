using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

using MongoDB.Bson.Serialization;

namespace pvhellouser.Controllers
{
    public class SidModel {
        public string sid {get;set;}
        public bool _permanent {get;set;}
    }

    [ApiController]
    [Route("/")]
    public class HelloUserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HelloUserController> _logger;

        public HelloUserController(ILogger<HelloUserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<SimpleReply> Get()
        {
            string sessionId = Request.Cookies["session"] as string;
            string json_b64 = sessionId.Substring(0, sessionId.IndexOf("."));
            byte[] json_bytes = Convert.FromBase64String(json_b64);
            SidModel sidmodel = 
                JsonSerializer.Deserialize<SidModel>(json_bytes);
            string sid = sidmodel.sid;
            
            ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect("redis:6379");
            IDatabase conn = muxer.GetDatabase();
            string ujson = await conn.StringGetAsync(sid);

            RedisUser user = BsonSerializer.Deserialize<RedisUser>(ujson);

            string hello = $"Hello {user.profile.username}";

            return new SimpleReply {
                Reply = hello
            };
        }
    }
}
