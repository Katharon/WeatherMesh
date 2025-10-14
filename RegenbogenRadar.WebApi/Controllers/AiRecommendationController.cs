namespace RegenbogenRadar.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OpenAI.Chat;
    using RegenbogenRadar.Shared.AiRecommendation;
    using System.Text;

    [Route("api/[controller]")]
    [ApiController]
    public class AiRecommendationController(ChatClient chatClient) : ControllerBase
    {
        private readonly ChatClient chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Post([FromBody] RecommendationRequestDto recommendationRequest, CancellationToken cancellationToken = default)
        {
            // Validate input
            if (recommendationRequest == null)
                return BadRequest("Fehlende Payload.");

            // Get data from payload
            var location = recommendationRequest.Location;
            var hourlyForecast = recommendationRequest.HourlyForecast;

            // Build prompt for AI
            var userPrompt = new StringBuilder();

            userPrompt.AppendLine($"Handle als Aktivitätsberater für Empfehlungen in '{location}'.");
            userPrompt.AppendLine($"Achte auf Wetter und Möglichkeiten zur aktuellen lokalen Uhrzeit inkl. Zeitspanne an genau diesem Ort.");
            userPrompt.AppendLine($"Antworte nur in prägnanten 2-3 Sätzen und schön formatiert.");
            userPrompt.AppendLine($"Ich bin eine API, du kannst keine Rückfragen stellen!");
            userPrompt.AppendLine($"Antworte so schnell wie möglich und in freundlicher Prosa!");
            foreach (var hour in hourlyForecast.Take(8))
            {
                userPrompt.AppendLine($"{hour.Time} - {hour.TemperatureC}°C - {hour.Summary}");
            }

            ChatCompletion completion = await this.chatClient.CompleteChatAsync(userPrompt.ToString());

            var recommendationText = completion.Content[0].Text.Trim();
            var recommendationDto = new RecommendationDto { Recommendation = recommendationText };
            return Ok(recommendationDto);
        }
    }
}
