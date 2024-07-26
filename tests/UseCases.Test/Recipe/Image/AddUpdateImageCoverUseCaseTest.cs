using Azure.Core;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Exceptions.ExceptionBase;
using MyRecipeBook.Exceptions;
using UseCases.Test.Recipe.InlineDatas;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using CommonTestUtilities.BlobStorage;
using MyRecipeBook.Domain.Entities;

namespace UseCases.Test.Recipe.Image;

public class AddUpdateImageCoverUseCaseTest
{
    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success(IFormFile file)
    {
        (var user, var _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Error_Recipe_NotFound(IFormFile file)
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(1, file);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) &&
                e.GetErrorMessages().Contains(ResourceMessagesException.Recipe_Not_Found));
    }

    [Fact]
    public async Task Error_File_Is_Txt()
    {
        (var user, var _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        var file = FormFileBuilder.Txt();

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) &&
                e.GetErrorMessages().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
    }

    public static AddUpdateImageCoverUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();


        return new AddUpdateImageCoverUseCase(repository, loggedUser, unitOfWork, blobStorage);
    }
}
