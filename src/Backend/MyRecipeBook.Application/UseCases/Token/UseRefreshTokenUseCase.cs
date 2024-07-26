using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Exceptions.ExceptionBase;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.ValueObjects;

namespace MyRecipeBook.Application.UseCases.Token;

public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refresthTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public UseRefreshTokenUseCase(ITokenRepository tokenRepository, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refresthTokenGenerator, IUnitOfWork unitOfWork)
    {
        _tokenRepository = tokenRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refresthTokenGenerator = refresthTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseTokens> Execute(RequestNewToken request)
    {
        var refreshToken = await _tokenRepository.Get(request.RefreshToken);

        if (refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(MyRecipeBookRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
        if(DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refresthTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

        await _unitOfWork.Commit();

        var teste = new ResponseTokens
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier),
            RefreshToken = newRefreshToken.Value
        };

        return teste;

    }
}
