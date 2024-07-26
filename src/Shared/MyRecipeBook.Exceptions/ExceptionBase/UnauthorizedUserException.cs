using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class UnauthorizedUserException : MyRecipeBookException
{
    public UnauthorizedUserException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
