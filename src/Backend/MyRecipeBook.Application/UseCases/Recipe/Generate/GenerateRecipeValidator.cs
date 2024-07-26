using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipe>
{
    public GenerateRecipeValidator()
    {
        var maximum_number_ingredients = MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;

        RuleFor(request => request.Ingredients.Count).InclusiveBetween(1, maximum_number_ingredients).WithMessage(ResourceMessagesException.Invalid_Number_Ingredients);
        RuleFor(request => request.Ingredients).Must(ingredients => ingredients.Count == ingredients.Distinct().Count()).WithMessage(ResourceMessagesException.Duplicated_Ingredients_In_List);
        RuleFor(request => request.Ingredients).ForEach(rule =>
        {
            rule.Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Ingredient", ResourceMessagesException.Ingredient_Empty);
                    return;
                }

                if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
                {
                    context.AddFailure("Ingredient", ResourceMessagesException.Ingredient_Not_Following_Pattern);
                    return;
                }
            });
        });
    }
}
