using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe;

public interface IRegisterRecipeUseCase
{
    public Task<ResponseRegisteredRecipe> Execute(RequestRegisterRecipeFormData request);
}
