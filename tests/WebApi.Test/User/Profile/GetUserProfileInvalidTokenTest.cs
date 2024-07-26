using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTokenTest : MyRecipeBookClassFixture
{
    private readonly string method = "api/user";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(method, "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Invalid()
    {
        var response = await DoGet(method, string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFoud()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(method, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
