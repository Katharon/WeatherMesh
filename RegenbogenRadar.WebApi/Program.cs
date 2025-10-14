namespace RegenbogenRadar.WebApi
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Web;
    using OpenAI.Chat;
    using Scalar.AspNetCore;
    using System.Globalization;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            // Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            // HttpClient für Geocoding
            builder.Services.AddHttpClient("GeocodingClient", client =>
            {
                client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/v1/");
            });

            // HttpClient für Wetterdaten
            builder.Services.AddHttpClient("WeatherForecastClient", client =>
            {
                client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
            });

            // HttpClient für AI-Recommendations
            builder.Services.AddSingleton<ChatClient>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var apiKey = config["AI_API_KEY"];
                var model = config["AI_MODEL"];

                if (string.IsNullOrWhiteSpace(apiKey))
                    throw new InvalidOperationException("API Key fehlt in Secrets.json");
                if (string.IsNullOrWhiteSpace(model))
                    throw new InvalidOperationException("API Model fehlt in Secrets.json");

                return new ChatClient(model, apiKey);
            });

            // Frontend auf anderem Origin => CORS aktivieren
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DevCors", policy =>
                {
                    policy.WithOrigins("https://localhost:7018")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();
            
            app.MapDefaultEndpoints();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseCors("DevCors");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
