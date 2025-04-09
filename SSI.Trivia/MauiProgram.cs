using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SSI.Trivia.Shared.DbContexts;

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
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Trivia.db");
            options.UseSqlite($"Filename={dbPath}");
        });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddMudServices();

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TriviaDbContext>();

        AddMigrationIfNeeded(dbContext);
        dbContext.Database.Migrate();

        return app;
    }

    private static void AddMigrationIfNeeded(TriviaDbContext dbContext)
    {
        var migrator = dbContext.GetService<IMigrator>();
        var migrationsAssembly = dbContext.GetService<IMigrationsAssembly>();

        var appliedMigrations = dbContext.Database.GetAppliedMigrations().ToList();
        var allMigrations = migrationsAssembly.Migrations.Keys.ToList();

        var pendingMigrations = allMigrations.Except(appliedMigrations).ToList();

        if (pendingMigrations.Count == 0) return;
        foreach (var migration in pendingMigrations)
        {
            migrator.Migrate(migration);
        }
    }
}