namespace MyRecipeBook.Communication.Response;

public class ResponseShortRecipe
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int AmountIngredients { get; set; }
    public string? ImageUrl { get; set; }
}
