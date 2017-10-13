using System;
using System.Collections.Concurrent;
using System.Web.Http;

namespace Demo.CancelOperation.WebApi.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        private const int Interval = 5;
        private const int Count = 20;

        private static readonly ConcurrentDictionary<Guid, ValuesRepository> Requests = 
            new ConcurrentDictionary<Guid, ValuesRepository>();

        // GET api/values/start
        [HttpGet]
        [Route("start")]
        public IHttpActionResult Start()
        {
            var key = Guid.NewGuid();
            var repo = new ValuesRepository(3, 20);
            Requests.TryAdd(key,repo);
            repo.Start();
            return Ok(key);
        }

        // GET api/values/progress/key
        [HttpGet]
        [Route("progress/{key}")]
        public IHttpActionResult Progress(Guid key)
        {
            if (!Requests.TryGetValue(key, out var repo))
            {
                return BadRequest($"Invalid key: {key}");
            }
            var progress = repo.Progress();
            return Ok(progress);
        }

        // GET api/values/cancel/key
        [HttpGet]
        [Route("cancel/{key}")]
        public IHttpActionResult Cancel(Guid key)
        {
            if (!Requests.TryGetValue(key, out var repo))
            {
                return BadRequest($"Invalid key: {key}");
            }
            repo.Cancel();
            return Ok();
        }

        // GET api/values/result/key
        [HttpGet]
        [Route("result/{key}")]
        public IHttpActionResult Result(Guid key)
        {
            if (!Requests.TryGetValue(key, out var repo))
            {
                return BadRequest($"Invalid key: {key}");
            }
            var result = repo.GetResult();
            return Ok(result);
        }
    }
}
