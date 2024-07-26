using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IGetDashboardUseCase
{
    public Task<ResponseRecipes> Execute();
}
