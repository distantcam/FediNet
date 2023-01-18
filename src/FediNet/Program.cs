using System.Text.Json;
using System.Text.Json.Serialization;
using FediNet.Extensions;
using FediNet.Infrastructure;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Serilog;

[assembly: MediatorOptions(ServiceLifetime = ServiceLifetime.Scoped)]

#pragma warning disable RS0030 // Do not used banned APIs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("FediNetDb");
    if (string.IsNullOrEmpty(connectionString))
        throw new Exception("Database connection string not set.");

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    // OpenApi
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(t => t.FullName?.Replace('+', '.'));
    });

    // Database
    //builder.Services
    //    .AddDbContext<FediNetContext>(options => options
    //        .UseSqlServer(connectionString, opts => opts
    //            .MigrationsAssembly(typeof(FediNetContext).Assembly.FullName)),
    //    optionsLifetime: ServiceLifetime.Singleton) // For scoped access use DbContext
    //    .AddDbContextFactory<FediNetContext>(lifetime: ServiceLifetime.Singleton); // For singleton access use DbContextFactory

    // Services
    builder.Services.AddBindingServices();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();
    builder.Services.AddMediator();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.Scan(scan => scan.FromAssemblyOf<Program>()
        // Register services
        .AddClasses(classes => classes.InNamespaces("FediNet.Services"))
            .AsImplementedInterfaces()
            .AsSelf()
            .WithScopedLifetime()
        // Register caches
        .AddClasses(classes => classes.InNamespaces("FediNet.Caching"))
            .AsSelf()
            .WithSingletonLifetime()
    );

    // Set up pipeline
    builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        // In development, create and migrate the database
        //var dbOptions = app.Services.GetRequiredService<DbContextOptions>();
        //new DbContext(dbOptions).Database.EnsureCreated();
        //using (var scope = app.Services.CreateScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<FediNetContext>();
        //    db.Database.Migrate();
        //}
    }
    else
    {
        app.UseHttpsRedirection();
    }
    app.UseSerilogRequestLogging();

    app.MapEndpoints();

    app.Run();
}
catch (HostAbortedException) { /* EF Migrations */ }
finally
{
    Log.CloseAndFlush();
}
