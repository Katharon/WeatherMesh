namespace RegenbogenRadar.Domain.WeatherForecast
{
    using System.Text.Json.Serialization;

    public class ForecastResponse
    {
        [JsonPropertyName("daily")]
        public DailyForecast? DailyForecast { get; set; }

        [JsonPropertyName("hourly")]
        public HourlyForecast? HourlyForecast { get; set; }
    }
}
