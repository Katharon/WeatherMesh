namespace RegenbogenRadar.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RegenbogenRadar.Domain;
    using OpenAI.Chat;
    using System.Text;

    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController(ChatClient chatClient) : ControllerBase
    {
        private readonly ChatClient chatClient = chatClient;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WeatherPayload payload, CancellationToken ct)
        {
            if (payload == null)
            {
                return BadRequest("Fehlende Payload.");
            }

            var userPrompt = new StringBuilder();
            userPrompt.AppendLine($"Was kann ich in {payload.Location} zur aktuellen lokalen Uhrzeit ({payload.TimeIso}) machen?");
            userPrompt.AppendLine("Beachte klimatische/geografische Bedingungen, die Tageszeit bei dem gegebenen Ort und dass es sich auch zetilich ausgeht.");
            userPrompt.AppendLine("Beispiel: Wandern nur, wenn Berg in der Nähe. Schwimmen nur, wenn Schwimmbad in der Nähe.");
            userPrompt.AppendLine("Empfehle keine gefährlichen Aktivitäten, wenn es z.B. Schneit, stürmt, gewittert, etc.");
            userPrompt.AppendLine("Welche Sehenswürdigkeiten, Veranstaltungen oder besonderen Orte gibt es in der Nähe des angegebenen Ortes?");
            userPrompt.AppendLine();
            userPrompt.AppendLine("Nach der Schematik sollst du arbeiten:");
            userPrompt.AppendLine("Sonnig, mild bis warm, bergige Region, Morgens => Geh doch wandern!");
            userPrompt.AppendLine("Regnerisch, kalt, trüb, ländlich, Abends => Mach dir einen schönen Netflix Abend.");
            userPrompt.AppendLine("Leicht regnerisch, mild, trüb, städtisch, Abends (17:30) => Geh ins Restaurant.");
            userPrompt.AppendLine();
            userPrompt.AppendLine("So wird das Wetter:");
            if (payload.Hourly != null)
            {
                foreach (var h in payload.Hourly.Take(16))
                {
                    userPrompt.AppendLine($"{h.Time} - {h.TempC}°C - {h.Summary}");
                }
            }
            userPrompt.AppendLine();
            userPrompt.AppendLine("Ich bin nur eine Anwendung auf einem Server und kommuniziere mit dir automatisiert über eine API.");
            userPrompt.AppendLine("Deshalb antworte bitte, als wärst du innerhalb einer App ein Aktivitätsberater für Freizeit.");
            userPrompt.AppendLine("Gib eine prägnante Empfehlung in 1-2 Sätzen mit maximal 3 alternativen Aktivitäten.");
            userPrompt.AppendLine("Antworte in natürlicher Sprache (Prosa)");

            ChatCompletion completion = await this.chatClient.CompleteChatAsync(userPrompt.ToString());
            Console.WriteLine(completion.Content[0].Text);

            return Ok(new { response = completion.Content[0].Text });
        }
    }
}
