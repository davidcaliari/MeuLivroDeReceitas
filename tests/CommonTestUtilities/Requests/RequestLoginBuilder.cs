using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginBuilder
{
    public static RequestLogin Build(int passwordLength = 10)
    {
        return new Faker<RequestLogin>()
            .RuleFor(login => login.Email, (f) => f.Internet.Email())
            .RuleFor(login => login.Password, (f) => f.Internet.Password(passwordLength));

    }
}
