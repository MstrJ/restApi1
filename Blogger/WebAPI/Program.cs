using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Confluent.Kafka;
using Domain.Interfaces;
using Infrastructure.Repositories;
using MongoDB.Driver;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Diagnostics;
using IHostedService = Microsoft.Extensions.Hosting.IHostedService;
using Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();



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


//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();

//private static IHostBuilder CreatehostBuilder(string[] args) =>
//Host.CreateDefaultBuilder(args)
//    .ConfigureServices((context, collection) =>
//    {
//        collection.AddHostedService<IHostedService>();
//    });

