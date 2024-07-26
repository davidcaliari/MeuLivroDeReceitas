using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Communication.Requests;

public class RequestRegisterRecipeFormData : RequestRecipe
{
    public IFormFile? Image { get; set; }
}
