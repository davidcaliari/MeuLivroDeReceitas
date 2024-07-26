using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionBase;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.API.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _accessTokenValidator = accessTokenValidator;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userIdentifier = _accessTokenValidator.ValidadeAndGetUserIdentifier(token);

            var exist = await _userReadOnlyRepository.ExistActiveUserWithIdentifier(userIdentifier);
            if (!exist)
            {
                throw new UnauthorizedUserException(ResourceMessagesException.User_Without_Permission_Access_Resource);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseError("TokenIsExpired")
            {
                TokenIsExpired = true
            });
        }
        catch (MyRecipeBookException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseError(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseError(ResourceMessagesException.User_Without_Permission_Access_Resource));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication))
        {
            throw new UnauthorizedUserException(ResourceMessagesException.No_Token);
        }

        return authentication["Bearer ".Length..].Trim();
    }
}
