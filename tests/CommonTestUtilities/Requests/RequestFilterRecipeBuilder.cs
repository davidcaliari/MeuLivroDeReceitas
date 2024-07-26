using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestFilterRecipeBuilder
{
    public static RequestFilterRecipe Build()
    {
        return new Faker<RequestFilterRecipe>()
            .RuleFor(r => r.RecipeTitle_Ingredient, f => f.Lorem.Word())
            .RuleFor(r => r.CookingTimes, f => f.Make(1, () => f.PickRandom<CookingTime>()))
            .RuleFor(r => r.Difficulties, f => f.Make(1, () => f.PickRandom<Difficulty>()))
            .RuleFor(r => r.DishTypes, f => f.Make(1, () => f.PickRandom<DishType>()));
    }
}
