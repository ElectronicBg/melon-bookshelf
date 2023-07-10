using MelonBookchelfApi.Infrastructure.Data;
using MelonBookchelfApi.Infrastructure.Repositories;
using MelonBookshelfApi.ApplicationExtentions;
using MelonBookshelfApi.MapperProfiles;
using MelonBookshelfApi.ProgramExtentions;
using MelonBookshelfApi.Services;
using MelonBookshelfApi.Services.Contracts;
using MelonBookshelfApi.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using IResourceService = MelonBookshelfApi.Services.Contracts.IResourceService;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DockerConnection");
builder.Services.AddDbContext<BookshelfDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<DbContext, BookshelfDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<BookshelfDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddLogging();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IHrActionsService, HrActionsService>();
builder.Services.AddScoped<IBaseUserActionsService, BaseUserAutomationService>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfiles>();
});

var swaggerSettings = builder.Configuration.GetSection("Swagger").Get<SwaggerSettings>();

builder.Services.AddSwagger(p =>
{
    p.LoadSettingsFrom(swaggerSettings!);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(swaggerSettings);
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Host terminated unexpectedly ({ApplicationName})...", appName);

    throw;
}

[ExcludeFromCodeCoverage]
public partial class Program
{
    public const string appName = "MelonBookshelfApi";
}
