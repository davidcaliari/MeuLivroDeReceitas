using Bogus;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.Dto;

public class GeneratedRecipeDtoBuilder
{
    public static GeneratedRecipeDto Build()
    {
        return new Faker<GeneratedRecipeDto>()
            .RuleFor(r => r.Title, faker => faker.Lorem.Word())
            .RuleFor(r => r.CookingTime, faker => faker.PickRandom<CookingTime>())
            .RuleFor(r => r.Ingredients, faker => faker.Make(1, () => faker.Commerce.ProductName()))
            .RuleFor(r => r.Instructions, faker => faker.Make(1, () => new GeneratedInstructionsDto
            {
                Step = 1,
                Text = faker.Lorem.Paragraph()
            }));
    }
}
