using BotDetect.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotDetect.Infrastructure.Data;

public class BotDetectDbContext : DbContext
{
    public BotDetectDbContext(DbContextOptions<BotDetectDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AnalysisHistory> AnalysisHistories => Set<AnalysisHistory>();
    public DbSet<AnalysisResult> AnalysisResults => Set<AnalysisResult>();
    public DbSet<BotRating> BotRatings => Set<BotRating>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<ExportedResult> ExportedResults => Set<ExportedResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.UserId);
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.Username).HasColumnName("username");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.Password).HasColumnName("password");
        });

        modelBuilder.Entity<Account>(e =>
        {
            e.ToTable("accounts");
            e.HasKey(x => x.AccountId);
            e.Property(x => x.AccountId).HasColumnName("account_id");
            e.Property(x => x.Username).HasColumnName("username");
            e.Property(x => x.SocialNetwork).HasColumnName("social_network");
            e.Property(x => x.Status).HasColumnName("status");
        });

        modelBuilder.Entity<AnalysisHistory>(e =>
        {
            e.ToTable("analysis_history");
            e.HasKey(x => x.HistoryId);
            e.Property(x => x.HistoryId).HasColumnName("history_id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.AnalysisDate).HasColumnName("analysis_date");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.SocialNetwork).HasColumnName("social_network");
            e.HasOne(x => x.User).WithMany(u => u.AnalysisHistories).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<BotRating>(e =>
        {
            e.ToTable("bot_ratings");
            e.HasKey(x => x.RatingId);
            e.Property(x => x.RatingId).HasColumnName("rating_id");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.RatingValue).HasColumnName("rating_value");
        });

        modelBuilder.Entity<AnalysisResult>(e =>
        {
            e.ToTable("analysis_results");
            e.HasKey(x => x.ResultId);
            e.Property(x => x.ResultId).HasColumnName("result_id");
            e.Property(x => x.HistoryId).HasColumnName("history_id");
            e.Property(x => x.AccountId).HasColumnName("account_id");
            e.Property(x => x.RatingId).HasColumnName("rating_id");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.AnalysisDate).HasColumnName("analysis_date");
            e.HasOne(x => x.AnalysisHistory).WithMany(h => h.AnalysisResults).HasForeignKey(x => x.HistoryId);
            e.HasOne(x => x.Account).WithMany(a => a.AnalysisResults).HasForeignKey(x => x.AccountId);
            e.HasOne(x => x.BotRating).WithMany(b => b.AnalysisResults).HasForeignKey(x => x.RatingId);
        });

        modelBuilder.Entity<Report>(e =>
        {
            e.ToTable("reports");
            e.HasKey(x => x.ReportId);
            e.Property(x => x.ReportId).HasColumnName("report_id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.ReportDate).HasColumnName("report_date");
            e.Property(x => x.FileFormat).HasColumnName("file_format");
            e.HasOne(x => x.User).WithMany(u => u.Reports).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<ExportedResult>(e =>
        {
            e.ToTable("exported_results");
            e.HasKey(x => x.ExportId);
            e.Property(x => x.ExportId).HasColumnName("export_id");
            e.Property(x => x.ReportId).HasColumnName("report_id");
            e.Property(x => x.FileLocation).HasColumnName("file_location");
            e.Property(x => x.Format).HasColumnName("format");
            e.Property(x => x.ExportDate).HasColumnName("export_date");
            e.HasOne(x => x.Report).WithMany(r => r.ExportedResults).HasForeignKey(x => x.ReportId);
        });
    }
}
