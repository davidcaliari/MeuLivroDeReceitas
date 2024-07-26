using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe;

public interface IGetRecipeByIdUseCase
{
    public Task<ResponseRecipe> Execute(long recipeId);
}
