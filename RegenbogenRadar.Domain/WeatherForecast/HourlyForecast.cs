namespace RegenbogenRadar.Domain.WeatherForecast
{
    using System.Text.Json.Serialization;

    public class HourlyForecast
    {
        [JsonPropertyName("time")]
        public string[]? Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double[]? Temperature2m { get; set; }

        [JsonPropertyName("weathercode")]
        public int[]? Weathercode { get; set; }
    }
}
