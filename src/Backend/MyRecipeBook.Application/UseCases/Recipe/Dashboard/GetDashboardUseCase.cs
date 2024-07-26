using AutoMapper;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.Recipe.Dashboard;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IBlobStorageService _blobStorageService;

    public GetDashboardUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository, IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
        _blobStorageService = blobStorageService;
    }

    public async Task<ResponseRecipes> Execute()
    {
        var loggedUser = await _loggedUser.User();

        var recipes = await _repository.GetForDashBoard(loggedUser);

        return new ResponseRecipes
        {
            Recipes = await recipes.MapToShortRecipe(loggedUser, _blobStorageService, _mapper)
            //Recipes = _mapper.Map<IList<ResponseShortRecipe>>(recipes)
        };
    }
}
