﻿using Moq;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Services.OpenAI;

namespace CommonTestUtilities.OpenAI;

public class GenerateRecipeAIBuilder
{
    public static IGenerateRecipeAI Build(GeneratedRecipeDto dto)
    {
        var mock = new Mock<IGenerateRecipeAI>();

        mock.Setup(service => service.Generate(It.IsAny<IList<string>>())).ReturnsAsync(dto);

        return mock.Object;
    }
}
