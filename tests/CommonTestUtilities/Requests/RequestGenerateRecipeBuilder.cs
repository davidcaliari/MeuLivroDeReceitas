using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestGenerateRecipeBuilder
{
    public static RequestGenerateRecipe Build(int count = 5)
    {
        return new Faker<RequestGenerateRecipe>()
            .RuleFor(user => user.Ingredients, faker => faker.Make(count, () => faker.Commerce.ProductName()));
    }
}
