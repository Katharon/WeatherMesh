namespace RegenbogenRadar.Shared.WeatherForecast
{
    using System;

    public class HourlyForecastDto
    {
        public required DateTime Time { get; init; }

        public required int TemperatureC { get; init; }

        public required string Summary { get; init; }
    }
}