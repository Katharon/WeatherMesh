namespace RegenbogenRadar.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RegenbogenRadar.Domain.WeatherForecast;
    using RegenbogenRadar.Shared.Geocoding;
    using RegenbogenRadar.Shared.WeatherForecast;
    using System.Globalization;
    using System.Text.Json;

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class WeatherForecastController(IHttpClientFactory httpClientFactory) : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        private static readonly JsonSerializerOptions jsonOptions = new (JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Post([FromBody] CoordinatesDto coordinates, CancellationToken cancellationToken = default)
        {
            // Validate
            if (coordinates.Latitude is < -90 or > 90 || coordinates.Longitude is < -180 or > 180)
                return BadRequest("Ungültige Koordinaten.");

            // Build URL for weather API
            var lat = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            var lon = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);

            Console.WriteLine("Debug:");
            Console.WriteLine(coordinates.Longitude);
            Console.WriteLine(coordinates.Latitude);

            var url = $"forecast?latitude={lat}&longitude={lon}" +
                      "&daily=temperature_2m_max,weathercode" +
                      "&hourly=temperature_2m,weathercode" +
                      "&timezone=auto&language=de&forecast_days=7";

            // Call weather API
            var client = httpClientFactory.CreateClient("WeatherForecastClient");

            using var response = await client.GetAsync(url, cancellationToken);
            var text = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, text);

            // Parse response
            ForecastResponse? data;
            try
            {
                data = JsonSerializer.Deserialize<ForecastResponse>(text, jsonOptions);
            }
            catch (JsonException ex)
            {
                return StatusCode(502, $"Provider lieferte ungültiges JSON: {ex.Message}");
            }

            if (data is null)
                return StatusCode(502, "Leere Antwort vom Vorhersage-Provider.");

            // Check for required data
            if (data.DailyForecast?.Time == null || data.DailyForecast.Temperature2mMax == null || data.DailyForecast.Weathercode == null ||
                data.HourlyForecast?.Time == null || data.HourlyForecast.Temperature2m == null || data.HourlyForecast.Weathercode == null)
            {
                return StatusCode(502, "Vorhersage unvollständig.");
            }

            // Mapping: Daily
            // Use the minimum length to avoid index issues
            var dailyForecastCount = new [] { data.DailyForecast.Time.Length, data.DailyForecast.Temperature2mMax.Length, data.DailyForecast.Weathercode.Length }.Min();
            var dailyForecastDto = new DailyForecastDto[dailyForecastCount];

            for (int i = 0; i < dailyForecastCount; i++)
            {
                var date = DateOnly.Parse(data.DailyForecast.Time[i], CultureInfo.InvariantCulture);
                var temperature = (int)Math.Round(data.DailyForecast.Temperature2mMax[i]);
                var summary = WeatherFromCode(data.DailyForecast.Weathercode[i]);
                dailyForecastDto[i] = new DailyForecastDto { Date = date, TemperatureC = temperature, Summary = summary };
            }

            // Mapping: Hourly
            // We only want today's hours
            var today = DateOnly.Parse(data.DailyForecast.Time[0], CultureInfo.InvariantCulture);
            var hourlyForecastCount = new[] { data.HourlyForecast.Time.Length, data.HourlyForecast.Temperature2m.Length, data.HourlyForecast.Weathercode.Length }.Min();
            var hourlyForecastDto = new List<HourlyForecastDto>(24);

            for (int i = 0; i < hourlyForecastCount; i++)
            {
                var timeString = DateTime.Parse(data.HourlyForecast.Time[i], CultureInfo.InvariantCulture);
                if (DateOnly.FromDateTime(timeString) == today)
                {
                    var temperature = (int)Math.Round(data.HourlyForecast.Temperature2m[i]);
                    var summary = WeatherFromCode(data.HourlyForecast.Weathercode[i]);
                    hourlyForecastDto.Add(new HourlyForecastDto { Time = timeString, TemperatureC = temperature, Summary = summary });
                }
            }

            // Return result
            var forecastDto = new ForecastDto { Daily = dailyForecastDto, Hourly = [.. hourlyForecastDto.OrderBy(h => h.Time)] };
            return Ok(forecastDto);
        }

        private static string WeatherFromCode(int code) => code switch
        {
            0               => "Klar",
            1 or 2          => "Überwiegend klar / teils bewölkt",
            3               => "Bedeckt",
            45 or 48        => "Nebel",
            51 or 53 or 55  => "Nieselregen",
            61 or 63 or 65  => "Regen",
            71 or 73 or 75  => "Schnee",
            80 or 81 or 82  => "Regenschauer",
            95              => "Gewitter",
            96 or 99        => "Gewitter mit Hagel",
            _               => "Unbekannt"
        };
    }
}
