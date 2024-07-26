using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.Login.External;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Application.UseCases.Recipe.Dashboard;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Application.UseCases.Token;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Delete.Delete;
using MyRecipeBook.Application.UseCases.User.Delete.Request;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Domain.Repositories.Recipe;
using Sqids;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            //.AddIdEncoder(configuration)
            .AddAutoMapper()
            .AddUserCases();

        return services;
    }

    private static IServiceCollection AddUserCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUserCase, RegisterUserUserCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUserCase, UpdateUserUserCase>();
        services.AddScoped<IChangePasswordUserCase, ChangePasswordUserCase>();
        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
        services.AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCase>();
        services.AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>();
        services.AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>();
        services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();
        services.AddScoped<IGenerateRecipeUseCase, GenerateRecipeUseCase>();
        services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
        services.AddScoped<IAddUpdateImageCoverUseCase, AddUpdateImageCoverUseCase>();
        services.AddScoped<IRequestDeleteUserUseCase, RequestDeleteUserUseCase>();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        services.AddScoped<IExternalLoginUseCase, ExternalLoginUseCase>();
        return services;
    }

    private static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        //services.AddAutoMapper(typeof(AutoMapping));
        services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOption =>
        {
            //var sqids = option.GetService<SqidsEncoder<long>>()!;

            autoMapperOption.AddProfile(new AutoMapping(/*sqids*/));
        }).CreateMapper());

        return services;
    }

    //private static IServiceCollection AddIdEncoder(this IServiceCollection services, IConfiguration configuration)
    //{
    //    var squids = new SqidsEncoder<long>(new()
    //    {
    //        //MinLength = 3,
    //        Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
    //    });
    //    services.AddSingleton(squids);
    //    return services;
    //}
}
