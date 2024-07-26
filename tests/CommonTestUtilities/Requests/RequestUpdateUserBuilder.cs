using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserBuilder
{
    public static RequestUpdateUser Build(int passwordLength = 10)
    {
        return new Faker<RequestUpdateUser>()
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name));
    }
}
