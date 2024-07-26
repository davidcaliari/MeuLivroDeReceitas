using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Response;

public class ResponseGeneratedRecipe
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<ResponseGenerateInstructions> Instructions { get; set; } = [];
    public CookingTime CookingTime { get; set; }
    public Difficulty Difficulty { get; set; }
}
