using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Exceptions.ExceptionBase;
using MyRecipeBook.Exceptions;

namespace UseCases.Test.Recipe.Update;

public class UpdateRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var request = RequestRecipeBuilder.Build();

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        (var user, var _) = UserBuilder.Build();

        var request = RequestRecipeBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(1000, request);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals(ResourceMessagesException.Recipe_Not_Found));
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, var _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var request = RequestRecipeBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) &&
                e.GetErrorMessages().Contains(ResourceMessagesException.Recipe_Title_Empty));
    }

    public static UpdateRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();


        return new UpdateRecipeUseCase(repository, loggedUser, unitOfWork, mapper);
    }
}
