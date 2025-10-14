namespace RegenbogenRadar.Domain.WeatherForecast
{
    using System.Text.Json.Serialization;

    public class DailyForecast
    {
        [JsonPropertyName("time")]
        public string[]? Time { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public double[]? Temperature2mMax { get; set; }

        [JsonPropertyName("weathercode")]
        public int[]? Weathercode { get; set; }
    }
}
