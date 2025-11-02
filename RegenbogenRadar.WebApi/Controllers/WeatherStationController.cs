namespace RegenbogenRadar.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class WeatherStationController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("{id}")] // Da noch timespan????
        public string Get(int id, TimeSpan timeSpan)
        {
            return "value";
        }

        [HttpGet("{id}")] // Da noch bool??? Gibt alle inaktiven Stationen zurück
        public string GetActive()
        {
            return "value";
        }

        [HttpPost]
        public void Post(Guid guid) // Gibt 
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}