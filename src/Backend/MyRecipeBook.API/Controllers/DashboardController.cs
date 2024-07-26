using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.API.Controllers;

[AuthenticatedUser]
public class DashboardController : MyRecipeBookBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipes), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetDashboardUseCase useCase)
    {
        var response = await useCase.Execute();

        if(response.Recipes.Any())
            return Ok(response);

        return NoContent();

    }
}
