namespace MyRecipeBook.Domain;

public interface IUnitOfWork
{
    public Task Commit();
}
