using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Token;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.API.Controllers;

public class TokenController : MyRecipeBookBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokens), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewToken request)
    {
        var response = useCase.Execute(request);
        return Ok(response);
    }
}
