using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// 
builder.Services.AddSingleton<IMongoClient, MongoClient>(x =>
{
    var uri = x.GetRequiredService<IConfiguration>()["MongoUri"];
    MongoClient client = new MongoClient(uri);
    return client;
});
builder.Services.AddScoped<IPostRepository,MongoRepository>();
builder.Services.AddScoped<IPostService, MongoService>();
builder.Services.AddSingleton(AutoMapperConfig.Initialize());

// pozwala dodac podpowiedzi co dana metoda robi
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });



var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
