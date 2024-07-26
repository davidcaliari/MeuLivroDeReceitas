using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.User.Register;

public class UpdateUserUserCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var request = RequestsRegisterUserBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestsRegisterUserBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) && e.GetErrorMessages().Contains(ResourceMessagesException.Name_Empty));
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestsRegisterUserBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) && e.GetErrorMessages().Contains(ResourceMessagesException.Email_Already_Registered));
    }

    private static RegisterUserUserCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();

        if (!string.IsNullOrEmpty(email))
            readRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new RegisterUserUserCase(writeRepository, readRepositoryBuilder.Build(), accessTokenGenerator, refreshTokenGenerator, tokenRepository, mapper, passwordEncripter, unitOfWork);
    }
}
