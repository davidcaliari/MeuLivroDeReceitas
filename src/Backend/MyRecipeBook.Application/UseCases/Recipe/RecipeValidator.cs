using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipe>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceMessagesException.Recipe_Title_Empty);
        RuleFor(recipe => recipe.CookingTime).IsInEnum();
        RuleFor(recipe => recipe.Difficulty).IsInEnum();
        RuleForEach(recipe => recipe.DishTypes).IsInEnum();

        RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0);
        RuleForEach(recipe => recipe.Ingredients).NotEmpty();

        RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0);
        RuleForEach(recipe => recipe.Instructions).NotEmpty();
        RuleForEach(recipe => recipe.Instructions).ChildRules(instructionRule =>
        {
            instructionRule.RuleFor(instruction => instruction.Step).GreaterThanOrEqualTo(1);
            
            instructionRule.RuleFor(instruction => instruction.Text)
            .NotEmpty()
            .MaximumLength(2000);
        });
        RuleFor(recipe => recipe.Instructions).Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count);
    }
}
