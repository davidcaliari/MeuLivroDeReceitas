using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Communication.Requests;
using Microsoft.Extensions.Options;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUserCase : IChangePasswordUserCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordUserCase(ILoggedUser loggedUser, IPasswordEncripter passwordEncripter, IUserUpdateOnlyRepository userUpdateOnlyRepository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _passwordEncripter = passwordEncripter;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestChangePassword request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);

        var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePassword request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        //Alterado devido ao BCrypt
        //var currentPasswordEncripted = _passwordEncripter.Encrypt(request.Password);
        //if (!currentPasswordEncripted.Equals(loggedUser.Password))
        //    result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.Password_Different_Current_Password));
        
        if (!_passwordEncripter.IsValid(request.Password, loggedUser.Password))
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.Password_Different_Current_Password));

        if(!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
