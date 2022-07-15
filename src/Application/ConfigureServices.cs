using System.Reflection;
using FinancialChat.Application;
using FinancialChat.Application.Common.Behaviours;
using FinancialChat.Application.MessageBroker;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.Configure<BotOptions>(configuration.GetSection(BotOptions.KEY));
        services.Configure<RabbitOptions>(configuration.GetSection(RabbitOptions.KEY));
        services.AddSingleton<IMessageProducer, RabbitMQProducer>();
        services.AddSingleton<IMessageConsumer, RabbitMQConsumer>();
        return services;
    }
}
