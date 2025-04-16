using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using SSI.Trivia.Shared.Configuration;
using SSI.Trivia.Shared.DbContexts;
using SSI.Trivia.Shared.Interfaces;
using SSI.Trivia.Shared.Services;

namespace SSI.Trivia;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddDbContext<TriviaDbContext>(options =>
        {
            // Get the path to the app's data directory
            var appDataDir = FileSystem.AppDataDirectory;
            var dbPath = Path.Combine(appDataDir, "Trivia.db");
            options.UseSqlite($"Filename={dbPath}");
        });
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddMudServices();
        builder.Services.Configure<OpenAISettings>(options =>
        {
            options.ApiKey = string.Empty;
        });
        builder.Services.AddHttpClient<IOpenAITriviaService, OpenAITriviaService>();
        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TriviaDbContext>();
        dbContext.Database.Migrate();

        Task.Run(async () =>
        {
            var apiKey = await SecureStorage.GetAsync("OpenAIApiKey") ?? string.Empty;
            var options = app.Services.GetRequiredService<IOptions<OpenAISettings>>().Value;
            options.ApiKey = apiKey;
        });

        return app;
    }
}