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

builder.Services.AddOpenApi();
builder.Services.Configure<JsonOptions>(o => { o.SerializerOptions.NumberHandling = JsonNumberHandling.Strict; });
builder.Services.AddCors(o => { o.AddPolicy("all", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });

builder.Services.AddScoped<TradeService>();
builder.Services.AddScoped<BookmarkService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<RealDataService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/{documentName}.json");
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/v1.json", "v1"); });
}

app.UseCors("all");

app.MapTradesApi();
app.MapStatisticsApi();
app.MapBookmarksApi();
app.MapCurrencyApi();

app.Run();