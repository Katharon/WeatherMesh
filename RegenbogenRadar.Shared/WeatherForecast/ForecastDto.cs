namespace RegenbogenRadar.Shared.WeatherForecast
{
    public class ForecastDto
    {
        public required DailyForecastDto[] Daily { get; init; }

        public required HourlyForecastDto[] Hourly { get; init; }
    }
}