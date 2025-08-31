using System.Reflection;
using System.Text.Json.Serialization;
using Dashboard.API;
using Dashboard.Services;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var path = builder.Configuration.GetValue<string>("VaultConfig");
if (!string.IsNullOrWhiteSpace(path))
    builder.Configuration.AddJsonFile(path, optional: true, reloadOnChange: false);

builder.Services.AddInvestApiClient((provider, settings) =>
    settings.AccessToken = provider.GetRequiredService<IConfiguration>().GetValue<string>("TOKEN"));

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, context, token) =>
    {
        var nullabilityInfoContext = new NullabilityInfoContext();
        foreach (var property in context.JsonTypeInfo.Type.GetProperties())
        {
            if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null) continue;
            var jsonName = property.Name;
            if (property.GetCustomAttribute<JsonPropertyNameAttribute>() is { } attr)
                jsonName = attr.Name;
            var str = schema.Properties?.Keys.SingleOrDefault(key => string.Equals(key, jsonName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(str) && nullabilityInfoContext.Create(property).ReadState != NullabilityState.Nullable)
            {
                schema.Required ??= [];
                schema.Required.Add(str);
            }
        }
        return Task.CompletedTask;
    });
});
builder.Services.Configure<JsonOptions>(o => { o.SerializerOptions.NumberHandling = JsonNumberHandling.Strict; });
builder.Services.AddCors(o => { o.AddPolicy("all", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });

builder.Services.AddScoped<TradeService>();
builder.Services.AddScoped<BookmarkService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<RealDataService>();

var app = builder.Build();

app.MapOpenApi("/{documentName}.json");
app.UseSwaggerUI(options => { options.SwaggerEndpoint("/v1.json", "v1"); });

app.UseCors("all");

app.MapTradesApi();
app.MapStatisticsApi();
app.MapBookmarksApi();
app.MapCurrencyApi();

app.Run();