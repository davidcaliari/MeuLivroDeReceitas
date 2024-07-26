﻿using Azure.Messaging.ServiceBus;
using MyRecipeBook.Application.UseCases.User.Delete.Delete;
using MyRecipeBook.Application.UseCases.User.Delete.Request;
using MyRecipeBook.Infrastructure.Services.ServiceBus;

namespace MyRecipeBook.API.BackgroundServices;

public class DeleteUserService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceBusProcessor _processor;

    public DeleteUserService(IServiceProvider serviceProvider, DeleteUserProcessor processor)
    {
        _serviceProvider = serviceProvider;
        _processor = processor.GetProcessor();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ExceptionReceivedHandler;

        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
    {
        var message = eventArgs.Message.Body.ToString();

        var userIdentifier = Guid.Parse(message);

        var scope = _serviceProvider.CreateScope();

        var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();

        await deleteUserUseCase.Execute(userIdentifier);
        
    }

    private Task ExceptionReceivedHandler(ProcessErrorEventArgs _) => Task.CompletedTask;

    ~DeleteUserService() => Dispose();

    public override void Dispose()
    {
        base.Dispose();

        GC.SuppressFinalize(this);
    }
}
