namespace RegenbogenRadar.Shared.WeatherForecast
{
    using System;

    public class DailyForecastDto
    {
        public required DateOnly Date { get; init; }

        public required int TemperatureC { get; init; }

        public required string Summary { get; init; }
    }
}