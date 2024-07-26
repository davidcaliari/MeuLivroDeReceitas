using Moq;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;

public class RecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _recipeReadOnlyRepositoryMock;

    public RecipeReadOnlyRepositoryBuilder() => _recipeReadOnlyRepositoryMock = new Mock<IRecipeReadOnlyRepository>();

    public RecipeReadOnlyRepositoryBuilder Filter(User user, IList<Recipe> recipes)
    {
        _recipeReadOnlyRepositoryMock.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);
        return this;
    } 
    
    public RecipeReadOnlyRepositoryBuilder GetForDashboard(User user, IList<Recipe>? recipes)
    {
        if(recipes is not null)
            _recipeReadOnlyRepositoryMock.Setup(repository => repository.GetForDashBoard(user)).ReturnsAsync(recipes);
        return this;
    }

    public RecipeReadOnlyRepositoryBuilder GetById(User user, Recipe? recipe)
    {
        if(recipe is not null)
            _recipeReadOnlyRepositoryMock.Setup(repository => repository.GetById(user, recipe.Id)).ReturnsAsync(recipe);
        return this;
    }

    public IRecipeReadOnlyRepository Build() => _recipeReadOnlyRepositoryMock.Object;
}
