using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.User.Register;

public interface IRegisterUserUserCase
{
    public Task<ResponseRegisterUser> Execute(RequestsRegisterUser request);
}
