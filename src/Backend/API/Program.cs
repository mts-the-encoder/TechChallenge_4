using Api.Filters;
using Api.Filters.Swagger;
using Application;
using Application.Services.Mapper;
using Domain.Extension;
using HashidsNet;
using Infrastructure;
using Infrastructure.Migrations;
using Infrastructure.RepositoryAccess;
using Serilog;
using static Serilog.Log;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<HashidsOperationFilter>();
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tech Challenge", Version = "1.0" });
    option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header with o Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });
    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Host.UseSerilog();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(opt => opt.Filters.Add(typeof(ExceptionsFilter)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguration(provider.GetService<IHashids>()));
}).CreateMapper());

builder.Services.AddScoped<AuthenticatedUserAttribute>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

UpdateDb();
AddSerilog();

app.Run();

void UpdateDb()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

    bool? databaseInMemory = context?.Database.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        var connection = builder.Configuration.GetConnection();
        var db = builder.Configuration.GetDb();
        Database.CreateDb(connection, db);
        
        app.MigrateDb();
    }
}

void AddSerilog()
{
	Logger = new LoggerConfiguration()
		.MinimumLevel.Debug()
		.WriteTo.Console()
		.CreateLogger();
}

public partial class Program { }