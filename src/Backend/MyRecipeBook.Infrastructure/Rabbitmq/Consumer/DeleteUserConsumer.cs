using MassTransit;
using MyRecipeBook.Domain;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.Rabbitmq.Consumer;

public class DeleteUserConsumer : IConsumer<User>
{
    private readonly IUserDeleteOnlyRepository _userDeleteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserConsumer(IUserDeleteOnlyRepository userDeleteOnlyRepository, IUnitOfWork unitOfWork)
    {
        _userDeleteOnlyRepository = userDeleteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<User> context)
    {
        await _userDeleteOnlyRepository.DeleteAccount(context.Message.UserIdentifier);
        await _unitOfWork.Commit();
    }
}
