using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class RefreshTokenNotFoundException : MyRecipeBookException
{
    public RefreshTokenNotFoundException() : base(ResourceMessagesException.User_Without_Permission_Access_Resource)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
