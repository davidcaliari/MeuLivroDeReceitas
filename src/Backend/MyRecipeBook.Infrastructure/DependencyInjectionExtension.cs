using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain;
using MyRecipeBook.Domain.MessageBus;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Rabbitmq;
using MyRecipeBook.Infrastructure.Rabbitmq.Consumer;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Security.Tokens.Refresh;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.OpenAI;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;
using OpenAI_API;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddTokens(configuration)
            .AddLoggedUser()
            .AddPasswordEncripter()
            .AddOpenAI(configuration)
            .AddAzureStorage(configuration)
            .AddQueue(configuration)
            .AddRabbitMq(configuration);


        if (configuration.IsUnityTestEnviroment())
            return services;

        services
            .AddDbContext_SqlServer(configuration)
            .AddFluentMigrator_SqlServer(configuration);

        return services;
    }

    private static IServiceCollection AddDbContext_SqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
            dbContextOptions.UseSqlServer(configuration.ConnectionString()));
        //dbContextOptions.UseSqlServer(configuration.ConnectionString(),
        //        o => o.UseQuerySplittingBehavior(
        //            QuerySplittingBehavior.SplitQuery)));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

        return services;
    }

    private static IServiceCollection AddFluentMigrator_SqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddSqlServer()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
        });

        return services;
    }

    private static IServiceCollection AddTokens(this IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));

        return services;
    }

    private static IServiceCollection AddLoggedUser(this IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();

        return services;
    }

    private static IServiceCollection AddPasswordEncripter(this IServiceCollection services)
    {
        //var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        //services.AddScoped<IPasswordEncripter>(option => new Sha512Encripter(additionalKey!));
        services.AddScoped<IPasswordEncripter, BCryptNet>();

        return services;
    }

    private static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, ChatGPTService>();

        var key = configuration.GetValue<string>("Settings:OpenAI:ApiKey");
        var authentication = new APIAuthentication(key);

        services.AddScoped<IOpenAIAPI>(option => new OpenAIAPI(authentication));

        return services;
    }

    private static IServiceCollection AddAzureStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");
        
        if(!string.IsNullOrEmpty(connectionString))
            services.AddScoped<IBlobStorageService>(c => new AzureStorageService(new BlobServiceClient(connectionString)));

        return services;
    }

    private static IServiceCollection AddQueue(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");

        if (string.IsNullOrEmpty(connectionString))
            return services;

        var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });

        var deleteQueue = new DeleteUserQueue(client.CreateSender("user"));

        var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1
        }));

        services.AddSingleton(deleteUserProcessor);

        services.AddScoped<IDeleteUserQueue>(option => deleteQueue);

        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("Settings:RabbitMq:Host") ?? throw new ArgumentNullException("Settings:RabbitMq:Host");
        var username = configuration.GetValue<string>("Settings:RabbitMq:Username") ?? throw new ArgumentNullException("Settings:RabbitMq:Username");
        var password = configuration.GetValue<string>("Settings:RabbitMq:Password") ?? throw new ArgumentNullException("Settings:RabbitMq:Password");

        services.AddMassTransit(busConfigurator =>
        {

            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<DeleteUserConsumer>();
            var entryAssembly = Assembly.GetExecutingAssembly();

            busConfigurator.AddConsumers(entryAssembly);

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(host), "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IMessageBus, MessageBus>();

        return services;
    }
}
