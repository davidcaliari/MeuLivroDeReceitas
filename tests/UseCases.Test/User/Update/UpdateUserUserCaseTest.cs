using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.User.Update;

public class UpdateUserUserCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) 
                && e.GetErrorMessages().Contains(ResourceMessagesException.Name_Empty));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) 
                && e.GetErrorMessages().Contains(ResourceMessagesException.Email_Already_Registered));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    private static UpdateUserUserCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
    {
        
        
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggerUser = LoggedUserBuilder.Build(user);

        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        if (!string.IsNullOrEmpty(email))
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new UpdateUserUserCase(loggerUser, userUpdateOnlyRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
    }
}
