namespace RegenbogenRadar.Blazor
{
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using RegenbogenRadar.Blazor.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient<BackendService>(client =>
            {
                // Adress must be the same as the WebAPI project
                var apiBase = new Uri("https://localhost:7219/");

                client.BaseAddress = apiBase;
            });

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            await builder.Build().RunAsync();
        }
    }
}
