namespace MyRecipeBook.Communication.Requests;

public class RequestGenerateRecipe
{
    public IList<string> Ingredients { get; set; } = [];
}
