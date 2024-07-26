using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IRefreshTokenGenerator _refresthTokenGenerator;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoLoginUseCase(IUserReadOnlyRepository repository, IAccessTokenGenerator accessTokenGenerator, IPasswordEncripter passwordEncripter, IRefreshTokenGenerator refresthTokenGenerator, ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncripter = passwordEncripter;
        _refresthTokenGenerator = refresthTokenGenerator;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseRegisterUser> Execute(RequestLogin request)
    {
        //Alterado a forma de garantir o usuário logado com a senha correta
        //var encriptedPassword = _passwordEncripter.Encrypt(request.Password);
        //var user = await _repository.GetUserByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

        var user = await _repository.GetByEmail(request.Email);

        if(user is null || !_passwordEncripter.IsValid(request.Password, user.Password))
            throw new InvalidLoginException();

        var refreshToken = await CreateAndSaveRefreshToken(user);

        return new ResponseRegisterUser
        {
            Name = user.Name,
            Tokens = new ResponseTokens
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
                RefreshToken = refreshToken,
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refresthTokenGenerator.Generate(),
            UserId = user.Id
        };

        await _tokenRepository.SaveNewRefreshToken(refreshToken);

        await _unitOfWork.Commit();

        return refreshToken.Value;
    }
}
