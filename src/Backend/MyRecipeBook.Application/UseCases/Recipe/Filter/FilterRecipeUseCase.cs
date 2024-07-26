using AutoMapper;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase : IFilterRecipeUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly IBlobStorageService _blobStorageService;

    public FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository recipeReadOnlyRepository, IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _blobStorageService = blobStorageService;
    }

    public async Task<ResponseRecipes> Execute(RequestFilterRecipe request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var filters = new Domain.Dto.FilterRecipesDto
        {
            RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
            CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
            Difficulties = request.Difficulties.Distinct().Select(c => (Domain.Enums.Difficulty)c).ToList(),
            DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.DishType)c).ToList(),
        };

        var recipes = await _recipeReadOnlyRepository.Filter(loggedUser, filters);

        return new ResponseRecipes
        {
            Recipes = await recipes.MapToShortRecipe(loggedUser, _blobStorageService, _mapper)
            //Recipes = _mapper.Map<IList<ResponseShortRecipe>>(recipes)
        };
    }

    private static void Validate(RequestFilterRecipe request)
    {
        var validator = new FilterRecipeValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
