using BotDetect.Application.Services;
using BotDetect.Domain.Repositories;
using BotDetect.Domain.Services;
using BotDetect.Infrastructure.Data;
using BotDetect.Infrastructure.Repositories;
using BotDetect.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"] ?? "http://localhost:3000", "http://frontend:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BotDetectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default") ??
        "Host=localhost;Database=botdetect;Username=postgres;Password=postgres"));

builder.Services.AddScoped<IAnalysisHistoryRepository, AnalysisHistoryRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAnalysisResultRepository, AnalysisResultRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IBotRatingRepository, BotRatingRepository>();
builder.Services.AddSingleton<IAnalysisQueue, InMemoryAnalysisQueue>();

builder.Services.AddScoped<IAnalysisService, AnalysisService>();
builder.Services.AddScoped<IAnalysisResultService, AnalysisResultService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();

/// <summary>Точка входа для интеграционных тестов (WebApplicationFactory).</summary>
public partial class Program { }
