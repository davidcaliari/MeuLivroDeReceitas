using MassTransit;
using MyRecipeBook.Domain.MessageBus;

namespace MyRecipeBook.Infrastructure.Rabbitmq;

public class MessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageBus(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public async Task PublishAsync<T>(T message, CancellationToken ct = default)
        where T : class => await _publishEndpoint.Publish(message, ct);
}
