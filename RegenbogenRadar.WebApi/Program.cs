namespace RegenbogenRadar.WebApi
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Abstractions;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.Resource;
    using OpenAI;
    using OpenAI.Chat;
    using OpenAI.Responses;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            // Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            // HttpClient for OpenAI
            //builder.Services.AddHttpClient("OpenAiClient", client =>
            //{
            //    client.BaseAddress = new Uri("https://api.openai.com/");
            //    client.Timeout = TimeSpan.FromSeconds(60); // Globaler Timeout.
            //});


            // OpenAI-Client in DI registrieren
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
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
