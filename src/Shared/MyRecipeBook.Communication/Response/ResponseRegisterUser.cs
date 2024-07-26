namespace MyRecipeBook.Communication.Response;

public class ResponseRegisterUser
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokens Tokens { get; set; } = default!;
}
