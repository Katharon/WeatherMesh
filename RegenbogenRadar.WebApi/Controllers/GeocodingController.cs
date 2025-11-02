namespace RegenbogenRadar.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RegenbogenRadar.Domain.Geocoding;
    using RegenbogenRadar.Shared.Geocoding;
    using System.Net.Http;
    using System.Text.Json;

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class GeocodingController(IHttpClientFactory httpClientFactory) : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        private static readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Post([FromBody] string name, CancellationToken cancellationToken = default)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name darf nicht leer sein.");

            // Call geocoding API
            var client = httpClientFactory.CreateClient("GeocodingClient");
            var url = $"search?name={Uri.EscapeDataString(name)}&count=1&language=de&format=json";

            // Send request
            using var response = await client.GetAsync(url, cancellationToken);
            var text = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, text);

            // Parse response
            GeocodingResponse? data;
            try
            {
                data = JsonSerializer.Deserialize<GeocodingResponse>(text, jsonOptions);
            }
            catch (JsonException ex)
            {
                return StatusCode(502, $"Geocoding-Provider lieferte ungültiges JSON: {ex.Message}");
            }

            // Extract coordinates
            var place = data?.Results?.FirstOrDefault();
            if (place == null)
                return NotFound($"Keinen Ort mit dem Namen '{name}' gefunden.");

            // Return coordinates
            var coordinatesDto = new CoordinatesDto { Latitude = place.Latitude, Longitude = place.Longitude };
            return Ok(coordinatesDto);
        }
    }
}
