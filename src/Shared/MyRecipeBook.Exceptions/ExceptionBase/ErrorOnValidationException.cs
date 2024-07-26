using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    //Refatorado para não quebrar o pricipio de aberto e fechado do solid lá na ExceptionFilter
    //public IList<string> ErrorMessages { get; set; }

    //public ErrorOnValidationException(IList<string> erros) : base(string.Empty)
    //{
    //    ErrorMessages = erros;
    //}

    private readonly IList<string> _errorMessages;

    public ErrorOnValidationException(IList<string> erros) : base(string.Empty)
    {
        _errorMessages = erros;
    }

    public override IList<string> GetErrorMessages() => _errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
