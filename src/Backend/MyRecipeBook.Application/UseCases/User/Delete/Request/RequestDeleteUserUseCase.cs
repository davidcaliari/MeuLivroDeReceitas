using MyRecipeBook.Domain;
using MyRecipeBook.Domain.MessageBus;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.Application.UseCases.User.Delete.Request;

public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
{
    private readonly IDeleteUserQueue _queue;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _messageBus;

    public RequestDeleteUserUseCase(IDeleteUserQueue queue, IUserUpdateOnlyRepository userUpdateOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMessageBus messageBus)
    {
        _queue = queue;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.User();

        var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

        user.Active = false;
        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();

        //Retirando a postagem no ServiceBus da Azure
        //await _queue.SendMessage(loggedUser);

        await _messageBus.PublishAsync(loggedUser);
    }
}
