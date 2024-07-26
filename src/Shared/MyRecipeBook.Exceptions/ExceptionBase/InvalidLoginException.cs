using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class InvalidLoginException : MyRecipeBookException
{
    public InvalidLoginException() : base(ResourceMessagesException.Email_Or_Password_invalid) { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
