using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Update;

public interface IUpdateUserUserCase
{
    public Task Execute(RequestUpdateUser request);
}
