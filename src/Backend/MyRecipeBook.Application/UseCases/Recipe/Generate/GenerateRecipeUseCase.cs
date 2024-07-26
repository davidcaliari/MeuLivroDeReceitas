using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public class GenerateRecipeUseCase : IGenerateRecipeUseCase
{
    private readonly IGenerateRecipeAI _generator;

    public GenerateRecipeUseCase(IGenerateRecipeAI generator)
    {
        _generator = generator;
    }

    public async Task<ResponseGeneratedRecipe> Execute(RequestGenerateRecipe request)
    {
        Validate(request);

        var response = await _generator.Generate(request.Ingredients);

        return new ResponseGeneratedRecipe
        {
            Title = response.Title,
            Ingredients = response.Ingredients,
            CookingTime = (Communication.Enums.CookingTime)response.CookingTime,
            Difficulty = Communication.Enums.Difficulty.Low,
            Instructions = response.Instructions.Select(c => new ResponseGenerateInstructions
            {
                Step = c.Step,
                Text = c.Text
            }).ToList()
        };
    }

    public static void Validate(RequestGenerateRecipe request)
    {
        var result = new GenerateRecipeValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
