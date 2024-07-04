using Api.Controllers;
using Api.Filters;
using Application.UseCases.User.Create;
using Application.UseCases.User.UpdatePassword;
using Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : TechChallengeController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromServices] ICreateUserUseCase useCase, [FromBody] UserRequest request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut("update-password")]
    [ServiceFilter(typeof(AuthenticatedUserAttribute))]
    public async Task<IActionResult> Create([FromServices] IUpdatePasswordUseCase useCase,[FromBody] UpdatePasswordRequest request)
    {
        await useCase.Execute(request);

        return Ok();
    }
}