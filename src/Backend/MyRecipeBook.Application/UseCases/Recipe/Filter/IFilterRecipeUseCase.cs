using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public interface IFilterRecipeUseCase
{
    public Task<ResponseRecipes> Execute(RequestFilterRecipe request);
}
