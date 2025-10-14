namespace RegenbogenRadar.Blazor.Services
{
    using RegenbogenRadar.Shared.AiRecommendation;
    using RegenbogenRadar.Shared.Geocoding;
    using RegenbogenRadar.Shared.WeatherForecast;
    using System.Net.Http.Json;
    using System.Threading;

    public class BackendService(HttpClient httpClient)
    {
        private readonly HttpClient httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<CoordinatesDto?> GetGeocodeAsync(string location, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/Geocoding", location, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Fehler beim Abrufen der Geokodierung: {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<CoordinatesDto>(cancellationToken);
        }

        public async Task<ForecastDto?> GetWeatherForecastAsync(CoordinatesDto coordinates, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/WeatherForecast", coordinates, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Fehler beim Abrufen der Wettervorhersage: {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<ForecastDto>(cancellationToken);
        }

        public async Task<RecommendationDto?> GetAiRecommendationAsync(RecommendationRequestDto recommendationRequest, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("api/AiRecommendation", recommendationRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Fehler beim Abrufen der KI-Empfehlung: {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<RecommendationDto>(cancellationToken);
        }
    }
}
