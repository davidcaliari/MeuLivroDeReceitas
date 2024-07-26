using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfile> Execute();
}
