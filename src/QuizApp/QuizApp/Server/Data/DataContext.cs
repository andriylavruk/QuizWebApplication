using Microsoft.EntityFrameworkCore;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestParticipant> TestParticipants { get; set; }
    public DbSet<TestResult> TestResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestResult>()
            .HasOne(x => x.TestParticipant)
            .WithMany(x => x.TestResults)
            .HasForeignKey(x => x.TestParticipantId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
