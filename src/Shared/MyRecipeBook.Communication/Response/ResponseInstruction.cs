namespace MyRecipeBook.Communication.Response;

public class ResponseInstruction
{
    public string Id { get; set; } = string.Empty;
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
}
