using Application.Services.Cryptography;
using Application.Services.Token;
using Application.Services.UserSigned;
using Application.UseCases.Login.DoLogin;
using Application.UseCases.Movies.Create;
using Application.UseCases.Movies.Delete;
using Application.UseCases.Movies.GetAll;
using Application.UseCases.Movies.GetById;
using Application.UseCases.Movies.Update;
using Application.UseCases.User.Create;
using Application.UseCases.User.UpdatePassword;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAdditionalKeyPassword(services, configuration);
        AddTokenJWT(services, configuration);
        AddHashIds(services, configuration);
        AddUserSigned(services);
        AddUseCase(services);
    }

    private static void AddUserSigned(IServiceCollection services)
    {
        services.AddScoped<IUserSigned, UserSigned>();
    }

    private static void AddAdditionalKeyPassword(IServiceCollection services, IConfiguration configuration)
    {
        var value = configuration.GetValue<string>("Configurations:Password:AdditionalKey");
        services.AddScoped(opt => new Encryptor(value));
    }

    private static void AddTokenJWT(IServiceCollection services, IConfiguration configuration)
    {
        var sectionLifeTime = configuration.GetValue<string>("Configurations:Jwt:LifeTimeInMinutes");
        var sectionToken = configuration.GetValue<string>("Configurations:Jwt:TokenKey");

        services.AddScoped(opt => new TokenService(int.Parse(sectionLifeTime), sectionToken));
    }

    private static void AddHashIds(IServiceCollection services, IConfiguration configuration)
    {
        var salt = configuration.GetRequiredSection("Configurations:HashIds:Salt");
        services.AddHashids(setup =>
        {
            setup.Salt = salt.Value;
            setup.MinHashLength = 3;
        });
    }

    private static void AddUseCase(IServiceCollection services)
    {
	    services.AddScoped<ICreateUserUseCase, CreateUserUseCase>()
		    .AddScoped<ILoginUseCase, LoginUseCase>()
		    .AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>()
		    .AddScoped<IUpdateMovieUseCase, UpdateMovieUseCase>()
		    .AddScoped<IDeleteMovieUseCase, DeleteMovieUseCase>()
		    .AddScoped<IGetAllMovieUseCase, GetAllMoviesUseCase>()
		    .AddScoped<IGetByMovieUseCase, GetByIdMovieUseCase>()
		    .AddScoped<ICreateMovieUseCase, CreateMovieUseCase>();
    }
}
