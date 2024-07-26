namespace MyRecipeBook.Domain.MessageBus;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}
