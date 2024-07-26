using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain;
using MyRecipeBook.Exceptions.ExceptionBase;
using MyRecipeBook.Exceptions;
using Microsoft.AspNetCore.Http;
using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Application.Extensions;

namespace MyRecipeBook.Application.UseCases.Recipe.Image;

public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public AddUpdateImageCoverUseCase(IRecipeUpdateOnlyRepository repository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task Execute(long recipeId, IFormFile file)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.Recipe_Not_Found);

        var fileStream = file.OpenReadStream();

        (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();
        if (!isValidImage)
            throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}"; //como erá a busca da extensão do arquivo. Path.GetExtension(file.FileName)
            _repository.Update(recipe);
            await _unitOfWork.Commit();
        }

        await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);
    }
}
