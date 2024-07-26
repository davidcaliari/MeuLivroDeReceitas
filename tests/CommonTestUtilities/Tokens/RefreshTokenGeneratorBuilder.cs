using Moq;
using MyRecipeBook.Domain.Security.Tokens;

namespace CommonTestUtilities.Tokens;

public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new Mock<IRefreshTokenGenerator>().Object;
}
