using Application.Services.Token;
using Domain.Entities;
using Domain.Repositories.User;
using Microsoft.AspNetCore.Http;

namespace Application.Services.UserSigned;

public class UserSigned : IUserSigned
{
    private readonly IHttpContextAccessor _accessor;
    private readonly TokenService _tokenService;
    private readonly IUserReadOnlyRepository _repository;

    public UserSigned(IHttpContextAccessor accessor, TokenService tokenService, IUserReadOnlyRepository repository)
    {
        _accessor = accessor;
        _tokenService = tokenService;
        _repository = repository;
    }

    public async Task<User> GetUser()
    {
        var authorization = _accessor.HttpContext.Request.Headers["Authorization"].ToString();

        var token = authorization["Bearer".Length..].Trim();

        var email = _tokenService.GetEmail(token);

        var user = await _repository.GetByEmail(email);

        return user;
    }
}