using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories;

public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _tokenRepository;

    public TokenRepositoryBuilder() => _tokenRepository = new Mock<ITokenRepository>();

    public TokenRepositoryBuilder Get(RefreshToken? refreshToken)
    {
        if(refreshToken is not null)
            _tokenRepository.Setup(r => r.Get(refreshToken.Value)).ReturnsAsync(refreshToken);
        
        return this;
    }

    public ITokenRepository Build() => _tokenRepository.Object;
}
