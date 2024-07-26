using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.API.Binders;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.API.Controllers
{
    [AuthenticatedUser]
    public class RecipeController : MyRecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredRecipe), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterRecipeUseCase useCase,
            [FromForm] RequestRegisterRecipeFormData request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResponseRecipes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Filter(
            [FromServices] IFilterRecipeUseCase useCase,
            [FromBody] RequestFilterRecipe request)
        {
            var response = await useCase.Execute(request);

            if (response.Recipes.Any())
                return Ok(response);

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRecipe), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetRecipeByIdUseCase useCase,
            [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);

        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRecipe), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteRecipeUseCase useCase,
            [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id)
        {
            await useCase.Execute(id);

            return NoContent();

        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRecipe), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IUpdateRecipeUseCase useCase,
            [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id,
            [FromBody] RequestRecipe request)
        {
            await useCase.Execute(id, request);

            return NoContent();

        }

        [HttpPost("generate")]
        [ProducesResponseType(typeof(ResponseGeneratedRecipe), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Generate(
            [FromServices] IGenerateRecipeUseCase useCase,
            [FromBody] RequestGenerateRecipe request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }

        [HttpPut]
        [Route("image/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateImage(
            [FromServices] IAddUpdateImageCoverUseCase useCase,
            [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id,
            IFormFile file)
        {
            await useCase.Execute(id, file);

            return NoContent();

        }
    }
}
