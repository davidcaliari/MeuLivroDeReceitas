using CommonTestUtilities.Dto;
using CommonTestUtilities.Entities;
using CommonTestUtilities.OpenAI;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.Recipe.Generate;

public class GenerateRecipeUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeBuilder.Build();

        var useCase = CreateUseCase(dto);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);
        result.CookingTime.Should().Be((MyRecipeBook.Communication.Enums.CookingTime)dto.CookingTime);
        result.Difficulty.Should().Be(MyRecipeBook.Communication.Enums.Difficulty.Low);

    }

    [Fact]
    public async void Error_Duplicated_Ingredients()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeBuilder.Build(4);
        request.Ingredients.Add(request.Ingredients[0]);

        var useCase = CreateUseCase(dto);

        var act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count.Equals(1) &&
                e.GetErrorMessages().Contains(ResourceMessagesException.Duplicated_Ingredients_In_List));
    }

    private static GenerateRecipeUseCase CreateUseCase(GeneratedRecipeDto dto)
    {
        var generateRecipeApi = GenerateRecipeAIBuilder.Build(dto);

        return new GenerateRecipeUseCase(generateRecipeApi);
    }
}
