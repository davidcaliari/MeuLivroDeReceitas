using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordBuilder
{
    public static RequestChangePassword Build(int passwordLength = 10)
    {
        return new Faker<RequestChangePassword>()
            .RuleFor(c => c.Password, (f) => f.Internet.Password())
            .RuleFor(c => c.NewPassword, (f) => f.Internet.Password(passwordLength));
    }
}
