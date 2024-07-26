using AutoMapper;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IBlobStorageService _blobStorageService;

    public GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository, IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
        _blobStorageService = blobStorageService;
    }

    public async Task<ResponseRecipe> Execute(long recipeId)
    {
        var loggerUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggerUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.Recipe_Not_Found);

        var response = _mapper.Map<ResponseRecipe>(recipe);

        if(!string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            var url = await _blobStorageService.GetFileUrl(loggerUser, recipe.ImageIdentifier);

            response.ImageUrl = url;
        }
        return response;
    }
}
