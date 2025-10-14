namespace RegenbogenRadar.Shared.AiRecommendation
{
    using RegenbogenRadar.Shared.WeatherForecast;

    public class RecommendationRequestDto
    {
        public required string Location { get; set; }

        public required HourlyForecastDto[] HourlyForecast { get; set; }
    }
}
