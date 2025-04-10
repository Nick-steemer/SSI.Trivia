using Microsoft.EntityFrameworkCore;
using SSI.Trivia.Shared.Models;

namespace SSI.Trivia.Shared.DbContexts;

public class TriviaDbContext(DbContextOptions<TriviaDbContext> options) : DbContext(options)
{
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<ParticipantSprint> ParticipantSprints { get; set; }
    public DbSet<ParticipantAnswer> ParticipantAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<ParticipantAnswer>()
            .HasOne(pa => pa.Participant)
            .WithMany(p => p.Answers)
            .HasForeignKey(pa => pa.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ParticipantAnswer>()
            .HasOne(pa => pa.Question)
            .WithMany()
            .HasForeignKey(pa => pa.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ParticipantAnswer>()
            .HasOne(pa => pa.SelectedAnswer)
            .WithMany()
            .HasForeignKey(pa => pa.SelectedAnswerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ParticipantSprint>()
            .HasOne(ps => ps.Participant)
            .WithMany(p => p.Sprints)
            .HasForeignKey(ps => ps.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ParticipantSprint>()
            .HasOne(ps => ps.Sprint)
            .WithMany()
            .HasForeignKey(ps => ps.SprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}