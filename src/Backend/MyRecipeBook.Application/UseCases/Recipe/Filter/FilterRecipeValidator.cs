using FluentValidation;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipe>
{
    public FilterRecipeValidator()
    {
        RuleForEach(filter => filter.CookingTimes).IsInEnum();
        RuleForEach(filter => filter.Difficulties).IsInEnum();
        RuleForEach(filter => filter.DishTypes).IsInEnum();
    }
}
