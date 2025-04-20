using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SSI.Trivia.Shared.DbContexts;

public class TriviaDbContextFactory : IDesignTimeDbContextFactory<TriviaDbContext>
{
    public TriviaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TriviaDbContext>();
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Trivia.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");

        return new TriviaDbContext(optionsBuilder.Options);
    }
}

