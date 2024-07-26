
using MyRecipeBook.Domain;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.User.Delete.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly IUserDeleteOnlyRepository _userDeleteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public DeleteUserAccountUseCase(IUserDeleteOnlyRepository userDeleteOnlyRepository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _userDeleteOnlyRepository = userDeleteOnlyRepository;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task Execute(Guid userIdentifier)
    {
        await _blobStorageService.DeleteContainer(userIdentifier);

        await _userDeleteOnlyRepository.DeleteAccount(userIdentifier);

        await _unitOfWork.Commit();
    }
}
