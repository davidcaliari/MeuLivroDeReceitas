using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestsRegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.Name_Empty);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.Email_Empty);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestsRegisterUser>());
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email).EmailAddress();
        });
    }
}
