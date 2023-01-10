using System.Reflection;
using SlimMessageBus.Host.Memory;
using SlimMessageBus;
using SlimMessageBus.Host.AspNetCore;
using Twitter.Domain.Handlers;
using Twitter.DataConsumer;
using Twitter.Domain.Events;
using Twitter.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<Twitter.Bus.IBus, Twitter.Bus.Bus>();


builder.Services.AddSingleton<IWriteRepository, InMemoryWriteRepository>();
builder.Services.AddSingleton<IReadRepository, InMemoryReadRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpClient<PollerBackgroundService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddHostedService<PollerBackgroundService>();

builder.Services.AddSlimMessageBus(mbb=>
{
    mbb
     .Produce<TweetsReceived>(x => x.DefaultTopic("tweets-received-topic"))
     .Consume<TweetsReceived>(x => x
               .Topic("tweets-received-topic")
               .WithConsumer<AllTweetsHandler>())
          .Consume<TweetsReceived>(x => x
             .Topic("tweets-received-topic")
              .WithConsumer<HashTagStatsHandler>())
          .Consume<TweetsReceived>(x => x
             .Topic("tweets-received-topic")
              .WithConsumer<LanguageHandler>())
          .Consume<TweetsReceived>(x => x
             .Topic("tweets-received-topic")
              .WithConsumer<AuthorHandler>())


    .WithProviderMemory(new MemoryMessageBusSettings
    {
        // Do not serialize the domain events and rather pass the same instance across handlers
        EnableMessageSerialization = false
    });
},
   addConsumersFromAssembly: new[] { Assembly.GetExecutingAssembly() }, // auto discover consumers and register into DI (see next section)
   addInterceptorsFromAssembly: new[] { Assembly.GetExecutingAssembly() }, // auto discover interceptors and register into DI (see next section)
   addConfiguratorsFromAssembly: new[] { Assembly.GetExecutingAssembly() } // auto discover modular configuration and register into DI (see next section)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
