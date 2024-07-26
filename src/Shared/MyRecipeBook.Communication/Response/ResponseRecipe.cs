using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Response;

public class ResponseRecipe
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public IList<ResponseIngredient> Ingredients { get; set; } = [];
    public IList<ResponseInstruction> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
    public CookingTime CookingTime { get; set; }
    public Difficulty Difficulty { get; set; }
    public string? ImageUrl { get; set; }
}
