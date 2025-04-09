using Microsoft.EntityFrameworkCore;
using SSI.Trivia.Shared.Models;

namespace SSI.Trivia.Shared.DbContexts;

public class TriviaDbContext(DbContextOptions<TriviaDbContext> options) : DbContext(options)
{
    public DbSet<Sprint> Games { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
}
