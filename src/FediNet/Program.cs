using Autofac;
using Autofac.Extensions.DependencyInjection;
using FediNet;
using FediNet.Extensions;
using Serilog;

#pragma warning disable RS0030 // Do not used banned APIs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilog();

    builder.Services.AddControllers(options =>
    {
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    Startup.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

    // Register our modules
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterAssemblyModules(typeof(Program).Assembly));

    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        // see https://github.com/dotnet/runtime/issues/60600
        throw;
    }

    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}
