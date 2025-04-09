using Microsoft.EntityFrameworkCore;
using SSI.Trivia.Shared.Models;

namespace SSI.Trivia.Shared.DbContexts;

public class TriviaDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    public TriviaDbContext(DbContextOptions<TriviaDbContext> options)
        : base(options)
    {
    }
}
