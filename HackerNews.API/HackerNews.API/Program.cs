using HackerNews.Models.Options;
using HackerNews.Repository.Implementation;
using HackerNews.Repository.Interfaces;
using HackerNews.Services.Implementations;
using HackerNews.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Runtime;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // url of front end app
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "HackerNews API",
        Version = "v1"
    });
});
builder.Services.Configure<HackerNewsOptions>(
builder.Configuration.GetSection(nameof(HackerNewsOptions)));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IHackerNewsRepository, HackerNewsRepository>();
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
builder.Services.AddHttpClient<IHackerNewsRepository, HackerNewsRepository>((provider, client) =>
{
    var options = provider.GetRequiredService<IOptions<HackerNewsOptions>>().Value;
    client.BaseAddress = new Uri(options.Url);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HackerNews API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();
