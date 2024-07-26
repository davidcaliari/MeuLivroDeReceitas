using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Token;

public interface IUseRefreshTokenUseCase
{
    public Task<ResponseTokens> Execute(RequestNewToken request);
}
