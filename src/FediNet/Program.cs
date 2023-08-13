using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// OpenApi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Fixes schema ids for nested types
    c.CustomSchemaIds(t => t.FullName?.Replace('+', '.'));
});

builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddMediator();

// Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddFediNet();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapEndpoints(); // TODO Source generator

app.Run();
