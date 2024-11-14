using GithubAnalyzeAPI.Data;
using GithubAnalyzeAPI.Data.Extensions;
using GithubAnalyzeAPI.Extensions;
using GithubAnalyzeAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"]!;
    options.UseMySQL(connectionString);
});

builder.Services
    .AddRefitClient<IGithubService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["GithubConfig:BaseAddress"]!));

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ITrafficService, TrafficService>();

builder.Services.AddJob();

var policyName = builder.Configuration.GetSection("CorsConfig:PolicyName").Get<string>()!;
var origins = builder.Configuration.GetSection("CorsConfig:Domains").Get<string[]>()!;

builder.Services.AddCors(options =>
{
    options.AddPolicy(policyName,
        b => b
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();

app.UseCors(policyName);

await app.InitialiseDatabaseAsync();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapApis();

app.Run();