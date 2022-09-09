using FinancialChat.Worker;
using Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<RabbitOptions>(configuration.GetSection(RabbitOptions.KEY));
        services.AddHttpClient();
        services.AddHostedService<RabbitMQWorker>();
    })
    .Build();

await host.RunAsync();

