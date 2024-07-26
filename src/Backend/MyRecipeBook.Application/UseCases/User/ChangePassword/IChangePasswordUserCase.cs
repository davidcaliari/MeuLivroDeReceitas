using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUserCase
{
    public Task Execute(RequestChangePassword request);
}
