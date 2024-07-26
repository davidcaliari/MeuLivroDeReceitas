using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public interface IGenerateRecipeUseCase
{
    public Task<ResponseGeneratedRecipe> Execute(RequestGenerateRecipe request);
}
