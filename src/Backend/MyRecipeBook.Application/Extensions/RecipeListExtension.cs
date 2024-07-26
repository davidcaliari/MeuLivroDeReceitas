using AutoMapper;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.Extensions;

public static class RecipeListExtension
{
    public static async Task<IList<ResponseShortRecipe>> MapToShortRecipe(
        this IList<Recipe> recipes,
        User user,
        IBlobStorageService blobStorageService,
        IMapper mapper)
    {
        var resul = new List<ResponseShortRecipe>();

        var result = recipes.Select(async recipe =>
        {
            var response = mapper.Map<ResponseShortRecipe>(recipe); ;

            if(!string.IsNullOrEmpty(recipe.ImageIdentifier))
                response.ImageUrl = await blobStorageService.GetFileUrl(user, recipe.ImageIdentifier);

            return response;
        });

        var response = await Task.WhenAll(result);
        return response;
    }
}
