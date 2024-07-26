using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class RefreshTokenExpiredException : MyRecipeBookException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.REFRESH_TOKEN_EXPIRED)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
