
using MyRecipeBook.Domain;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public DeleteRecipeUseCase(ILoggedUser loggedUser, IRecipeWriteOnlyRepository recipeWriteOnlyRepository, IRecipeReadOnlyRepository recipeReadOnlyRepository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _loggedUser = loggedUser;
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();
        
        var recipe = await _recipeReadOnlyRepository.GetById(loggedUser, recipeId);
        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.Recipe_Not_Found);

        if(recipe.ImageIdentifier is not null)
            await _blobStorageService.Delete(loggedUser, recipe.ImageIdentifier);

        await _recipeWriteOnlyRepository.Delete(recipeId);

        await _unitOfWork.Commit();
    }
}
