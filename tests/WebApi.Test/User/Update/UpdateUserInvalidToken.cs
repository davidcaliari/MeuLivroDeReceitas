using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidToken : MyRecipeBookClassFixture
{
    private readonly string method = "api/user";

    public UpdateUserInvalidToken(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserBuilder.Build();

        var response = await DoPut(method, request, "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Invalid()
    {
        var request = RequestUpdateUserBuilder.Build();

        var response = await DoPut(method, request, string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFoud()
    {
        var request = RequestUpdateUserBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
