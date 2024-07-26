namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    //Não precisa mais com a nova forma de encriptar usando o BCryptNet
    //public Task<Entities.User?> GetUserByEmailAndPassword(string email, string password);

    public Task<bool> ExistActiveUserWithEmail(string email);

    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);

    public Task<Entities.User?> GetByEmail(string email);

}
